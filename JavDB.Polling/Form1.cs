using JavDB.Film;
using JavDB.Film.Common;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace JavDB.Polling
{
    public partial class Form1 : Form
    {
        private Config mConfig;
        private Grappler mGrap;
        private List<FilmInformation>? mFilms;
        private bool mPolling = false;
        private int mPageValue = 1;
        private int mPage
        {
            get => mPageValue;
            set
            {
                if (value == 1)
                {
                    btnPageUp.Enabled = false;
                }
                else
                {
                    btnPageUp.Enabled = true;
                }
                mPageValue = value;
                txtPageValue.Text = value.ToString();
            }
        }
        /// <summary>
        /// 确保文件路径已存在
        /// </summary>
        /// <param name="dirpath"></param>
        /// <returns></returns>
        [DllImport("dbghelp.dll", EntryPoint = "MakeSureDirectoryPathExists", CallingConvention = CallingConvention.StdCall)]
        public static extern long MakeSureDirectoryPathExists(string dirpath);
        public Form1()
        {
            InitializeComponent();
            try
            {

                mGrap = new Grappler(Grappler.ReadFile(Application.StartupPath + "JavDB.Film.json"));
                Config? config = JsonSerializer.Deserialize<Config>(Grappler.ReadFile(Application.StartupPath + "JavDB.Polling.json"), Grappler.SerializerOptions);
                if (config != null)
                {
                    mConfig = config;
                }
                else
                {
                    throw new Exception("加载配置文件失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mConfig = new Config();
                mGrap = new Grappler("{}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSeries.Text = mConfig.Series;
            numSize.Value = mConfig.Size;
            txtStart.Text = mConfig.Start.ToString();
            txtEnd.Text = mConfig.End.ToString();
            numDelay.Value = mConfig.Delay;
            numExpirationTime.Value = mConfig.ExpirationTime;
            txtURL.Text = mGrap.SRC;
            if (mConfig.Mode == 0)
            {
                rbStandard.Checked = true;
            }
            else if (mConfig.Mode == 1)
            {
                rbSimple.Checked = true;
            }
            else if (mConfig.Mode == 2)
            {
                rbFliterScore.Checked = true;
            }
            numScore.Enabled = rbFliterScore.Checked;
            numScore.Value = (decimal)mConfig.Score;
        }
        private delegate void progressValueChange(int value);
        private void progressValueChangeF(int value)
        {
            progressBar1.Value = value;
        }
        private delegate void addItem(int index, string uid, string state);
        private void addItemF(int index, string uid, string state)
        {
            ListViewItem item = new ListViewItem((index + 1).ToString());
            item.SubItems.Add(uid);
            item.SubItems.Add(string.IsNullOrEmpty(state) ? "抓取中" : state);
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.Selected = true;
            listInfo.Items.Add(item);
            listInfo.EnsureVisible(index);
        }
        private delegate void updateItem(int index, string state, string score, string date, string title, string actors, string url, string flag);
        private void updateItemF(int index, string state, string score, string date, string title, string actors, string url, string flag)
        {
            listInfo.Items[index].SubItems[2].Text = state;
            listInfo.Items[index].SubItems[3].Text = score;
            listInfo.Items[index].SubItems[4].Text = date;
            listInfo.Items[index].SubItems[5].Text = title;
            listInfo.Items[index].SubItems[6].Text = actors;
            listInfo.Items[index].SubItems[7].Text = url;
            listInfo.Items[index].SubItems[8].Text = flag;
            listInfo.Items[index].Selected = true;
            listInfo.EnsureVisible(index);
        }

        private void btnNextBatch_Click(object sender, EventArgs e)
        {
            try
            {
                int Start = int.Parse(txtStart.Text);
                int End = int.Parse(txtEnd.Text);
                txtStart.Text = (End + 1).ToString();
                txtEnd.Text = (End + End - Start + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnStartPolling_Click(object sender, EventArgs e)
        {
            try
            {
                mConfig.Series = txtSeries.Text.ToUpper();
                mConfig.Size = (int)numSize.Value;
                mConfig.Start = int.Parse(txtStart.Text);
                mConfig.End = int.Parse(txtEnd.Text);
                mConfig.Delay = (int)numDelay.Value;
                mConfig.Mode = (byte)(rbStandard.Checked ? 0 : rbSimple.Checked ? 1 : 2);
                mConfig.Score = (double)numScore.Value;
                mConfig.ExpirationTime = (ushort)numExpirationTime.Value;
                mConfig.Save(Application.StartupPath + "JavDB.Polling.json");

                int len = 1 + mConfig.End - mConfig.Start;
                if (len > 0)
                {
                    listInfo.Items.Clear();
                    mFilms = new List<FilmInformation>();
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    Action<bool> startPolling = (value) =>
                    {
                        tabControl1.Enabled = !value;
                        listInfo.Enabled = !value;
                        groupBox1.Enabled = tabControl1.Enabled;
                    };
                    new Thread(() =>
                    {
                        mPolling = true;
                        this.Invoke(startPolling, true);
                        for (int i = 0; i < len; i++)
                        {
                            if (mPolling == false) break;
                            int auid = i + mConfig.Start;
                            string uid = $"{mConfig.Series}-{auid.ToString().PadLeft(mConfig.Size, '0')}";
                            this.Invoke(new addItem(addItemF), i, uid, "抓取中");
                            FilmInformation? film = new FilmInformation();
                            try
                            {
                                string file = Path.Combine(mConfig.CachePath, uid.Split("-")[0], uid, uid + ".json");
                                if (File.Exists(file))
                                {
                                    StreamReader sr = File.OpenText(file);
                                    string cache = sr.ReadToEnd();
                                    sr.Close();
                                    film = FilmInformation.Convert(cache);
                                    if (film == null)
                                    {
                                        film = new FilmInformation();
                                        film.GrabTime = "1971-01-01 00:00:00";
                                    }
                                    DateTime dateTime;
                                    DateTime.TryParse(film.GrabTime, out dateTime);
                                    if (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds)
                                    {
                                        if (mConfig.Delay > 0 && i > 0)
                                            Thread.Sleep(mConfig.Delay);
                                        if (rbFliterScore.Checked)
                                        {
                                            film = mGrap.Grab(uid, (double)numScore.Value);
                                        }
                                        else
                                        {
                                            film = mGrap.Grab(uid, rbSimple.Checked);
                                        }
                                        string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!);
                                        File.WriteAllText(file, film.ToString());
                                        if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                    }
                                    else if (film.Actor.Count == 0 && film.Category.Count == 0)
                                    {
                                        if (rbStandard.Checked || (rbFliterScore.Checked && double.Parse(film.Score) >= (double)numScore.Value))
                                        {
                                            if (mConfig.Delay > 0 && i > 0)
                                                Thread.Sleep(mConfig.Delay);
                                            if (rbStandard.Checked)
                                            {
                                                film = mGrap.Grab(uid);
                                            }
                                            else
                                            {
                                                mGrap.GrabDetail(ref film);
                                            }
                                            string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!);
                                            File.WriteAllText(file, film.ToString());
                                            if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                        }
                                    }
                                }
                                else
                                {
                                    if (mConfig.Delay > 0 && i > 0)
                                        Thread.Sleep(mConfig.Delay);
                                    if (rbFliterScore.Checked)
                                    {
                                        film = mGrap.Grab(uid, (double)numScore.Value);
                                    }
                                    else
                                    {
                                        film = mGrap.Grab(uid, rbSimple.Checked);
                                    }
                                    if (film != null && film.UID == uid)
                                    {
                                        string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID);
                                        MakeSureDirectoryPathExists(file);
                                        File.WriteAllText(file, film.ToString());
                                        if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                    }
                                    else
                                    {
                                        throw new FileNotFoundException($"{uid} NOT FOUND");
                                    }

                                }
                                if (film != null)
                                {
                                    this.Invoke(new updateItem(updateItemF), i, "抓取成功", film.Score, film.Date, film.Title, film.Actor.GetActorNames(Gender.WOMAN).GetString(), film.Index, film.Category.ToArray().GetString(','));
                                }
                                else
                                {
                                    this.Invoke(new updateItem(updateItemF), i, "抓取失败", null, null, "Null of film", null, null, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Invoke(new updateItem(updateItemF), i, "抓取失败", null, null, ex.Message, null, null, null);
                            }
                            mFilms.Add(film);
                            this.Invoke(new progressValueChange(progressValueChangeF), (int)((double)(i + 1) / len * 100));
                        }
                        this.Invoke(startPolling, false);
                        mPolling = false;
                    })
                    { IsBackground = true }.Start();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(mGrap.SRC + this.listInfo.Items[nowIndex].SubItems[7].Text);
                    process.StartInfo = processStartInfo;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                menuItemActor.DropDownItems.Clear();
                if (mFilms == null) return;
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    if (mFilms[nowIndex].Actor.Count > 0)
                    {
                        int i = 1;
                        foreach (var act in mFilms[nowIndex].Actor)
                        {
                            ToolStripMenuItem item;
                            if (i >= 40)
                            {
                                item = new ToolStripMenuItem($"以及其他{mFilms[nowIndex].Actor.GetActorNames(Gender.WOMAN).Length - 40}位演员...");
                                item.Click += (_, _) =>
                                {
                                    MessageBox.Show(mFilms[nowIndex].Actor.GetActorNames(Gender.WOMAN).GetString(), "主演", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                };
                                menuItemActor.DropDownItems.Add(item);
                                break;
                            }
                            else
                            {
                                if (act.Gender != Gender.WOMAN) continue;
                                item = new ToolStripMenuItem(act.ToString());
                                item.Click += (_, _) =>
                                {
                                    tabControl1.SelectedIndex = 1;
                                    txtPage.Text = act.Url;
                                    loadPage(1);
                                };
                                menuItemActor.DropDownItems.Add(item);
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void menuItemPoster_Click(object sender, EventArgs e)
        {
            if (mFilms == null) return;
            SelectedListViewItemCollection s = this.listInfo.SelectedItems;
            if (s.Count > 0)
            {
                int nowIndex = s[0].Index;
                if (mFilms[nowIndex] != null && mFilms[nowIndex].Poster != null)
                {
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(mFilms[nowIndex].Poster!);
                    process.StartInfo = processStartInfo;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
            }
        }

        private void menuItemPreviewVideo_Click(object sender, EventArgs e)
        {
            if (mFilms == null) return;
            SelectedListViewItemCollection s = this.listInfo.SelectedItems;
            if (s.Count > 0)
            {
                int nowIndex = s[0].Index;
                if (mFilms[nowIndex] != null && mFilms[nowIndex].PreviewVideo != null)
                {
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo($"http://m.karevin.cn:8086/javdb/player.htm?url={mFilms[nowIndex].PreviewVideo}&muted=true&poster={mFilms[nowIndex].Poster}");
                    process.StartInfo = processStartInfo;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                else
                {
                    MessageBox.Show("未获取到预览视频地址！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void menuItemMagent_Click(object sender, EventArgs e)
        {
            SelectedListViewItemCollection s = this.listInfo.SelectedItems;
            if (s.Count > 0)
            {
                int nowIndex = s[0].Index;
                string file = Path.Combine(mConfig.CachePath, this.listInfo.Items[nowIndex].SubItems[1].Text.Split("-")[0], this.listInfo.Items[nowIndex].SubItems[1].Text, this.listInfo.Items[nowIndex].SubItems[1].Text + ".html");
                if (File.Exists(file))
                {
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(file);
                    process.StartInfo = processStartInfo;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                else
                {
                    MessageBox.Show("未获取到磁力链接信息！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void rbFliterScore_CheckedChanged(object sender, EventArgs e)
        {
            numScore.Enabled = rbFliterScore.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mPolling)
            {
                if (MessageBox.Show("操作正在进行中，是否取消？", "取消", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    mPolling = false;
                }
                e.Cancel = true;
            }
        }
        private void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtPage.Text))
            {
                loadPage(1);
            }
        }
        private void loadPage(int page)
        {
            try
            {
                listInfo.Items.Clear();
                mFilms = new List<FilmInformation>();
                progressBar1.Value = 0;
                Application.DoEvents();
                string url = $"{txtURL.Text}{txtPage.Text}";
                if (url.IndexOf("page=") >= 0)
                    throw new UriFormatException("地址参数不能包括page.");
                if (page > 1)
                {
                    if (url.IndexOf("?") >= 0)
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }
                    url += $"page={page}";
                }
                if (mGrap.GrabPage($"{url}", out List<FilmInformation> films))
                {
                    films.Sort();
                    mFilms = films;
                    Action<bool> startPolling = (value) =>
                    {
                        if (!value)
                        {
                            btnPageDown.Enabled = true;
                        }
                        tabControl1.Enabled = !value;
                        groupBox1.Enabled = tabControl1.Enabled;
                        listInfo.Enabled = !value;
                    };
                    new Thread(() =>
                    {
                        mPolling = true;
                        this.Invoke(startPolling, true);
                        for (int i = 0; i < films.Count; i++)
                        {
                            if (mPolling == false) break;
                            this.Invoke(new addItem(addItemF), i, films[i].UID, "抓取中");
                            try
                            {
                                string file = Path.Combine(mConfig.CachePath, films[i].SeriesNumber!, films[i].UID!, films[i].UID + ".json");
                                FilmInformation? film;
                                if (File.Exists(file))
                                {
                                    StreamReader sr = File.OpenText(file);
                                    string cache = sr.ReadToEnd();
                                    sr.Close();
                                    film = FilmInformation.Convert(cache);
                                    if (film == null)
                                    {
                                        film = films[i];
                                    }
                                    else
                                    {
                                        film.Score = films[i].Score;
                                    }
                                    DateTime dateTime;
                                    DateTime.TryParse(film.GrabTime, out dateTime);
                                    if (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds || (film.Actor.Count == 0 && film.Category.Count == 0))
                                    {
                                        if (rbStandard.Checked || (rbFliterScore.Checked && double.Parse(film.Score) >= (double)numScore.Value))
                                        {
                                            if (mConfig.Delay > 0 && i > 0)
                                                Thread.Sleep(mConfig.Delay);
                                            mGrap.GrabDetail(ref film);
                                            string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!);
                                            File.WriteAllText(file, film.ToString());
                                            if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                        }
                                    }
                                    films[i] = film;
                                }
                                else
                                {
                                    film = films[i];
                                    if (rbStandard.Checked || (rbFliterScore.Checked && double.Parse(film.Score) >= (double)numScore.Value))
                                    {
                                        if (mConfig.Delay > 0 && i > 0)
                                            Thread.Sleep(mConfig.Delay);
                                        mGrap.GrabDetail(ref film);
                                    }
                                    string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!);
                                    MakeSureDirectoryPathExists(file);
                                    File.WriteAllText(file, film.ToString());
                                    if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                }
                                if (film != null)
                                {
                                    this.Invoke(new updateItem(updateItemF), i, "抓取成功", film.Score, film.Date, film.Title, film.Actor.GetActorNames(Gender.WOMAN).GetString(), film.Index, film.Category.ToArray().GetString(','));
                                }
                                else
                                {
                                    this.Invoke(new updateItem(updateItemF), i, "抓取失败", null, null, "Null of film", null, null, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Invoke(new updateItem(updateItemF), i, "抓取失败", null, null, ex.Message, null, null, null);
                            }
                            this.Invoke(new progressValueChange(progressValueChangeF), (int)((double)(i + 1) / films.Count * 100));
                        }
                        this.Invoke(startPolling, false);
                        mPolling = false;
                    })
                    { IsBackground = true }.Start();
                }
                else
                {
                    btnPageDown.Enabled = false;
                    MessageBox.Show("未搜索到任何结果。", "搜索", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                mPage = page;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnLoadPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPage.Text))
            {
                loadPage(1);
            }
        }
        private void btnPageUp_Click(object sender, EventArgs e)
        {
            loadPage(mPage - 1);
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            loadPage(mPage + 1);
        }
        private void btnSaveUIDList_Click(object sender, EventArgs e)
        {
            try
            {
                if (mFilms == null) return;
                saveFileDialog1.Title = "保存文件";
                saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
                if (!string.IsNullOrEmpty(txtFileName.Text)) saveFileDialog1.FileName = txtFileName.Text;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    txtFileName.Text = fileName;
                    bool mode = false;
                    if (File.Exists(fileName))
                    {
                        var result = MessageBox.Show("文件已存在，请确认操作！\n\n是：追加到文件\n否：覆盖文件\n取消：取消操作", "注意", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Cancel)
                        {
                            return;
                        }
                        mode = result == DialogResult.Yes;
                    }
                    using (StreamWriter sr = new StreamWriter(fileName, mode))
                    {
                        foreach (var film in mFilms)
                        {
                            if (film.UID != null)
                            {
                                sr.WriteLine(film.UID);
                            }
                        }
                    }
                    MessageBox.Show("保存成功！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    listInfo.ContextMenuStrip = contextMenuStrip1;
            //}
            //else
            //{
            //    listInfo.ContextMenuStrip = contextMenuStrip1;
            //}
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "打开文件";
            openFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
            }
        }
        private void loadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                listInfo.Items.Clear();
                mFilms = new List<FilmInformation>();
                progressBar1.Value = 0;
                Application.DoEvents();
                Action<bool> startPolling = (value) =>
                {
                    tabControl1.Enabled = !value;
                    groupBox1.Enabled = tabControl1.Enabled;
                    btnSortScore.Enabled = !value;
                    listInfo.Enabled = !value;
                    lblFileCount.Text = $"共 {mFilms.Count} 条";
                };
                new Thread(() =>
                {
                    mPolling = true;
                    this.Invoke(startPolling, true);
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string[] buffer = sr.ReadToEnd().Split("\r\n");
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            string line = buffer[i].ToUpper();
                            if (!line.IsNullOrEmpty())
                            {
                                var film = new FilmInformation();
                                if (mPolling == false) break;
                                film.UID = line;
                                this.Invoke(new addItem(addItemF), i, line, "等待中");
                                mFilms.Add(film);
                            }
                            double ww = ((double)i + 1) / buffer.Length * 100;
                            this.Invoke(new progressValueChange(progressValueChangeF), (int)ww);
                        }
                        this.Invoke(startPolling, true);
                    }
                    for (int i = 0; i < mFilms.Count; i++)
                    {
                        if (mPolling == false) break;
                        this.Invoke(new updateItem(updateItemF), i, "抓取中", null, null, null, null, null, null);
                        FilmInformation? film = new FilmInformation();
                        try
                        {
                            string file = Path.Combine(mConfig.CachePath, mFilms[i].SeriesNumber!, mFilms[i].UID!, mFilms[i].UID + ".json");
                            if (File.Exists(file))
                            {
                                StreamReader sr = File.OpenText(file);
                                string cache = sr.ReadToEnd();
                                sr.Close();
                                film = FilmInformation.Convert(cache);
                                if (film == null)
                                {
                                    film = new FilmInformation();
                                    film.GrabTime = "1971-01-01 00:00:00";
                                }
                                DateTime dateTime;
                                DateTime.TryParse(film.GrabTime, out dateTime);
                                if (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds)
                                {
                                    if (mConfig.Delay > 0 && i > 0)
                                        Thread.Sleep(mConfig.Delay);
                                    if (rbFliterScore.Checked)
                                    {
                                        film = mGrap.Grab(mFilms[i].UID!, (double)numScore.Value);
                                    }
                                    else
                                    {
                                        film = mGrap.Grab(mFilms[i].UID!, rbSimple.Checked);
                                    }
                                    string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!);
                                    File.WriteAllText(file, film.ToString());
                                    if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                }
                                else if (film.Actor.Count == 0)
                                {
                                    if (rbStandard.Checked || (rbFliterScore.Checked && double.Parse(film.Score) >= (double)numScore.Value))
                                    {
                                        if (mConfig.Delay > 0 && i > 0)
                                            Thread.Sleep(mConfig.Delay);
                                        if (rbStandard.Checked)
                                        {
                                            film = mGrap.Grab(mFilms[i].UID!);
                                        }
                                        else
                                        {
                                            mGrap.GrabDetail(ref film);
                                        }
                                        string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!);
                                        File.WriteAllText(file, film.ToString());
                                        if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                    }
                                }
                            }
                            else
                            {
                                if (mConfig.Delay > 0 && i > 0)
                                    Thread.Sleep(mConfig.Delay);
                                if (rbFliterScore.Checked)
                                {
                                    film = mGrap.Grab(mFilms[i].UID!, (double)numScore.Value);
                                }
                                else
                                {
                                    film = mGrap.Grab(mFilms[i].UID!, rbSimple.Checked);
                                }
                                if (film != null && film.UID == mFilms[i].UID!)
                                {
                                    string path = Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID);
                                    MakeSureDirectoryPathExists(file);
                                    File.WriteAllText(file, film.ToString());
                                    if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                                }
                                else
                                {
                                    throw new FileNotFoundException($"{mFilms[i].UID!} NOT FOUND");
                                }

                            }
                            if (film != null)
                            {
                                mFilms[i] = film;
                                this.Invoke(new updateItem(updateItemF), i, "抓取成功", film.Score, film.Date, film.Title, film.Actor.GetActorNames(Gender.WOMAN).GetString(), film.Index, film.Category.ToArray().GetString(','));
                            }
                            else
                            {
                                this.Invoke(new updateItem(updateItemF), i, "抓取失败", null, null, "Null of film", null, null, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new updateItem(updateItemF), i, "抓取失败", null, null, ex.Message, null, null, null);
                        }
                        this.Invoke(new progressValueChange(progressValueChangeF), (int)((double)(i + 1) / mFilms.Count * 100));
                    }
                    mPolling = false;
                    this.Invoke(startPolling, false);
                })
                { IsBackground = true }.Start();
            }
        }

        private void btnStartPollingFile_Click(object sender, EventArgs e)
        {
            loadFile(txtFileName.Text);
        }
        private void btnSortScore_Click(object sender, EventArgs e)
        {
            if (mFilms == null) return;
            listInfo.Items.Clear();
            progressBar1.Value = 0;
            Application.DoEvents();
            Action<bool> startPolling = (value) =>
            {
                tabControl1.Enabled = !value;
                groupBox1.Enabled = tabControl1.Enabled;
                lblFileCount.Text = $"共 {mFilms.Count} 条";
            };
            Action<int, string, string, string, string, string, string, string, string> addItem = (index, uid, state, score, date, title, actors, url, flag) =>
            {
                ListViewItem item = new ListViewItem((index + 1).ToString());
                item.SubItems.Add(uid);
                item.SubItems.Add(state.IsNullOrEmpty() ? "抓取中" : state);
                item.SubItems.Add(score);
                item.SubItems.Add(date);
                item.SubItems.Add(title);
                item.SubItems.Add(actors);
                item.SubItems.Add(url);
                item.SubItems.Add(flag);
                item.Selected = true;
                listInfo.Items.Add(item);
                listInfo.EnsureVisible(index);
            };
            new Thread(() =>
            {
                mPolling = true;
                mFilms.Sort();
                this.Invoke(startPolling, true);
                for (int i = 0; i < mFilms.Count; i++)
                {
                    if (mPolling == false) break;
                    this.Invoke(addItem, i, mFilms[i].UID, mFilms[i].Index.IsNullOrEmpty() ? "抓取失败" : "抓取成功", mFilms[i].Score, mFilms[i].Date, mFilms[i].Title, mFilms[i].Actor.GetActorNames(Gender.WOMAN).GetString(), mFilms[i].Index, mFilms[i].Category.ToArray().GetString(','));
                    this.Invoke(new progressValueChange(progressValueChangeF), (int)((double)(i + 1) / mFilms.Count * 100));
                }
                mPolling = false;
                this.Invoke(startPolling, false);
            })
            { IsBackground = true }.Start();
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            btnStartPollingFile.Enabled = txtFileName.Text.Length > 0;
        }

        private void menuItemCopyUID_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    Clipboard.SetDataObject(this.listInfo.Items[nowIndex].SubItems[1].Text, true);
                    MessageBox.Show("复制成功！", "复制", MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void menuItemCopyActor_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    Clipboard.SetDataObject(this.listInfo.Items[nowIndex].SubItems[6].Text, true);
                    MessageBox.Show("复制成功！", "复制", MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtPageValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(txtPageValue.Text.Trim(), out int result))
                {
                    if (result < 1 || result > 99)
                    {
                        MessageBox.Show("页面错误，范围：1-99！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    txtPageValue.Text = result.ToString();
                    loadPage(result);
                }
                else
                {
                    txtPageValue.Text = mPage.ToString();
                }
            }
        }

        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            btnLoadPage.Enabled = txtPage.Text.Length > 0;
        }

        private void menuItemCopyJson_Click(object sender, EventArgs e)
        {
            try
            {
                if (mFilms == null) return;
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    if (mFilms[nowIndex] == null) return;
                    Clipboard.SetDataObject(mFilms[nowIndex].ToString(), true);
                    MessageBox.Show("复制成功！", "复制", MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
    public class ListViewEx : ListView
    {
        public ListViewEx()
        {
            SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}