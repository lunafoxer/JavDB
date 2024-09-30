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
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.ListView;

namespace JavDB.Polling
{
    public partial class Form1 : Form
    {
        private Config mConfig;
        private Grappler mGrap;
        private string m_Player = "file:///" + Path.Combine(Application.StartupPath, "player.htm");
        private List<FilmInformation>? mFilms;
        private List<CategoryEntity>? mCategory;
        private bool mPolling = false;
        private int mPageValue = 1;
        private string? m_PageUrl = null;
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
            }
            var cmp = new AutoCompleteStringCollection();
            cmp.AddRange(mConfig.AutoComplete);
            txtPageAuto.AutoCompleteCustomSource = cmp;
            txtPageManual.AutoCompleteCustomSource = cmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            numDelay.Value = mConfig.Delay;
            numExpirationTime.Value = mConfig.ExpirationTime;
            txtURL.Text = mGrap.SRC;
            txtURL2.Text = mGrap.SRC;
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
        private delegate void startPollingDelegate(bool value);
        private void startPolling(bool value)
        {
            if (!value)
            {
                btnPageDown.Enabled = true;
            }
            tabControl1.Enabled = !value;
            groupBox1.Enabled = tabControl1.Enabled;
            listInfo.Enabled = !value;
            btnSortScore.Enabled = !value;
            btnSaveUIDList.Enabled = !value;
            lblCount.Text = $"共 {(mFilms == null ? "0" : mFilms.Count)} 条";
            statusLabel.Text = value ? "正在加载" : "已就绪";
        }
        private delegate void changeStatusDelegate(string value);
        private void changeStatus(string value)
        {
            statusLabel.Text = value;
        }
        private delegate void progressValueChangeDelegate(int value);
        private void progressValueChange(int value)
        {
            progressBar1.Value = value;
        }
        private delegate void addItemDelegate(int index, string state, FilmInformation film);
        private void addItem(int index, string state, FilmInformation film)
        {
            if (film == null) return;
            ListViewItem item = new ListViewItem((index + 1).ToString());
            item.Tag = film;
            item.SubItems.Add(film.UID);
            item.SubItems.Add(string.IsNullOrEmpty(state) ? "抓取中" : state);
            item.SubItems.Add(film.Score);
            item.SubItems.Add(film.Date);
            item.SubItems.Add(film.Title);
            item.SubItems.Add(film.Actor.GetActorNames(Gender.WOMAN).GetString());
            item.SubItems.Add(film.Index);
            item.SubItems.Add(film.Category.GetString(','));
            item.Selected = true;
            listInfo.Items.Add(item);
            listInfo.EnsureVisible(index);
        }
        private delegate void updateItemDelegate(int index, string state, FilmInformation film);
        private void updateItem(int index, string state, FilmInformation film)
        {
            listInfo.Items[index].SubItems[2].Text = state;
            if (film != null)
            {
                listInfo.Items[index].Tag = film;
                if (!film.Score.IsNullOrEmpty()) listInfo.Items[index].SubItems[3].Text = film.Score;
                if (!film.Date.IsNullOrEmpty()) listInfo.Items[index].SubItems[4].Text = film.Date;
                if (!film.Title.IsNullOrEmpty()) listInfo.Items[index].SubItems[5].Text = film.Title;
                if (film.Actor.Count > 0) listInfo.Items[index].SubItems[6].Text = film.Actor.GetActorNames(Gender.WOMAN).GetString();
                if (!film.Index.IsNullOrEmpty()) listInfo.Items[index].SubItems[7].Text = film.Index;
                if (film.Category.Count > 0) listInfo.Items[index].SubItems[8].Text = film.Category.GetString(',');
            }
            listInfo.Items[index].Selected = true;
            listInfo.EnsureVisible(index);
        }

        private void listInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (mFilms == null) return;
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    FilmInformation film = (FilmInformation)s[0].Tag;
                    if (film == null || film.UID.IsNullOrEmpty()) return;
                    Process.Start(Application.StartupPath + "\\JavDB.Client.exe", film.UID!);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void initFilter()
        {
            menuItemFilter.DropDownItems.Clear();
            if (mFilms == null) return;
            if (mCategory != null)
            {
                ToolStripMenuItem item = new ToolStripMenuItem($"全部({mFilms.Count})");
                item.Click += (_, _) =>
                {
                    if (mFilms == null) return;
                    listInfo.Items.Clear();
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    new Thread(() =>
                    {
                        mPolling = true;
                        this.Invoke(new startPollingDelegate(startPolling), true);
                        int i = 0;
                        foreach (var it in mFilms)
                        {
                            if (mPolling == false) break;
                            this.Invoke(new addItemDelegate(addItem), i, it.Index.IsNullOrEmpty() ? "抓取失败" : "抓取成功", it);
                            i++;
                            this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)((double)i / mFilms.Count * 100));
                        }
                        mPolling = false;
                        this.Invoke(new startPollingDelegate(startPolling), false);
                    })
                    { IsBackground = true }.Start();
                };
                menuItemFilter.DropDownItems.Add(item);

                menuItemFilter.DropDownItems.Add(new ToolStripSeparator());

                foreach (var c in mCategory)
                {
                    item = new ToolStripMenuItem(c.ToString());
                    item.Click += (_, _) =>
                    {
                        if (mFilms == null) return;
                        listInfo.Items.Clear();
                        progressBar1.Value = 0;
                        Application.DoEvents();
                        new Thread(() =>
                        {
                            mPolling = true;
                            this.Invoke(new startPollingDelegate(startPolling), true);
                            int i = 0, j = 0;
                            foreach (var it in mFilms)
                            {
                                if (mPolling == false) break;
                                if (it.Category?.GetString(',')?.IndexOf(c.Name) >= 0)
                                {
                                    this.Invoke(new addItemDelegate(addItem), i, it.Index.IsNullOrEmpty() ? "抓取失败" : "抓取成功", it);
                                    i++;
                                }
                                j++;
                                this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)((double)j / mFilms.Count * 100));
                            }
                            mPolling = false;
                            this.Invoke(new startPollingDelegate(startPolling), false);
                        })
                        { IsBackground = true }.Start();

                    };
                    menuItemFilter.DropDownItems.Add(item);
                }
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
                    FilmInformation film = (FilmInformation)s[0].Tag;
                    if (film.Actor.Count > 0)
                    {
                        int i = 1;
                        foreach (var act in film.Actor)
                        {
                            ToolStripMenuItem item;
                            if (i >= 40)
                            {
                                item = new ToolStripMenuItem($"以及其他{film.Actor.GetActorNames(Gender.WOMAN).Length - 40}位演员...");
                                item.Click += (_, _) =>
                                {
                                    MessageBox.Show(film.Actor.GetActorNames(Gender.WOMAN).GetString(), "主演", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                    tabControl1.SelectedIndex = 2;
                                    txtPageAuto.Text = act.Url;
                                    loadPage(txtURL2.Text + act.Url, 1, true);
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

        private void menuItemBackdrop_Click(object sender, EventArgs e)
        {
            if (mFilms == null) return;
            SelectedListViewItemCollection s = this.listInfo.SelectedItems;
            if (s.Count > 0)
            {
                FilmInformation film = (FilmInformation)s[0].Tag;
                if (film != null && film.Backdrop != null)
                {
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(film.Backdrop!);
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
                FilmInformation film = (FilmInformation)s[0].Tag;
                if (film != null && film.PreviewVideo != null)
                {
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo($"{m_Player}?url={film.PreviewVideo}&muted=true&poster={film.Backdrop}");
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
                FilmInformation film = (FilmInformation)s[0].Tag;
                if (film == null) return;
                if (film.UID.IsNullOrEmpty()) return;
                string file = Path.Combine(mConfig.CachePath, film.UID!.Split("-")[0], film.UID, film.UID + ".html");
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
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtPageManual.Text))
            {
                loadPage($"{txtURL.Text}{txtPageManual.Text}", 1, false);
            }
        }
        private void btnLoadPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPageManual.Text))
            {
                loadPage($"{txtURL.Text}{txtPageManual.Text}", 1, false);
            }
        }
        private void btnPageUp_Click(object sender, EventArgs e)
        {
            mPage--;
            loadPage($"{txtURL.Text}{txtPageManual.Text}", mPage, false);
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            mPage++;
            loadPage($"{txtURL.Text}{txtPageManual.Text}", mPage, false);
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
                        if (mode != true)
                        {
                            sr.WriteLine("#Page: " + (tabControl1.SelectedIndex == 0 ? m_PageUrl : txtPageManual.Text));
                        }
                        foreach (ListViewItem it in listInfo.Items)
                        {
                            if (it.SubItems[1].Text != null)
                            {
                                sr.WriteLine(it.SubItems[1].Text);
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
                saveConfig();
                listInfo.Items.Clear();
                mFilms = new List<FilmInformation>();
                List<string> cate = new List<string>();
                progressBar1.Value = 0;
                Application.DoEvents();
                new Thread(() =>
                {
                    mPolling = true;
                    this.Invoke(new startPollingDelegate(startPolling), true);
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string[] buffer = sr.ReadToEnd().Split("\r\n");
                        m_PageUrl = "";
                        if (buffer.Length > 0)
                        {
                            if (buffer[0].StartsWith("#Page:"))
                            {
                                m_PageUrl = buffer[0].Substring(6, buffer[0].Length - 6).Trim();
                                if (m_PageUrl.Length > 0)
                                {
                                    Action<string> setUrl = (text) =>
                                    {
                                        txtPageManual.Text = text;
                                    };
                                    this.Invoke(setUrl, m_PageUrl);
                                }
                                buffer = buffer.Skip(1).Take(buffer.Length - 1).ToArray();
                            }
                        }
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            string line = buffer[i].ToUpper();
                            if (!line.IsNullOrEmpty())
                            {
                                var film = new FilmInformation();
                                if (mPolling == false) break;
                                film.UID = line;
                                this.Invoke(new addItemDelegate(addItem), i, "等待中", film);
                                mFilms.Add(film);
                            }
                            double ww = ((double)i + 1) / buffer.Length * 100;
                            this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)ww);
                        }
                        this.Invoke(new startPollingDelegate(startPolling), true);
                    }
                    for (int i = 0; i < mFilms.Count; i++)
                    {
                        if (mPolling == false) break;
                        this.Invoke(new updateItemDelegate(updateItem), i, "抓取中", null);
                        try
                        {
                            FilmInformation? film = mFilms[i];
                            if (grabFilm(ref film))
                            {
                                mFilms[i] = film;
                                cate.AddRange(film.Category);
                                this.Invoke(new updateItemDelegate(updateItem), i, "抓取成功", film);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new updateItemDelegate(updateItem), i, "抓取失败", new FilmInformation() { Title = ex.Message });
                        }
                        this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)((double)(i + 1) / mFilms.Count * 100));
                    }
                    mPolling = false;
                    mCategory = CategoryEntity.Parse(cate);
                    initFilter();
                    this.Invoke(new startPollingDelegate(startPolling), false);
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
            new Thread(() =>
            {
                mPolling = true;
                mFilms.Sort();
                this.Invoke(new startPollingDelegate(startPolling), true);
                for (int i = 0; i < mFilms.Count; i++)
                {
                    if (mPolling == false) break;
                    this.Invoke(new addItemDelegate(addItem), i, mFilms[i].Index.IsNullOrEmpty() ? "抓取失败" : "抓取成功", mFilms[i]);
                    this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)((double)(i + 1) / mFilms.Count * 100));
                }
                mPolling = false;
                this.Invoke(new startPollingDelegate(startPolling), false);
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
                    loadPage($"{txtURL.Text}{txtPageManual.Text}", result);
                    mPage = result;
                }
                else
                {
                    txtPageValue.Text = mPage.ToString();
                }
            }
        }

        private void txtPageManual_TextChanged(object sender, EventArgs e)
        {
            btnLoadPage.Enabled = txtPageManual.Text.Length > 0;
            if (txtPageAuto.Text.Length != txtPageManual.Text.Length || txtPageAuto.Text != txtPageManual.Text)
                txtPageAuto.Text = txtPageManual.Text;
        }
        private void txtPageAuto_TextChanged(object sender, EventArgs e)
        {
            if (txtPageAuto.Text.Length != txtPageManual.Text.Length || txtPageAuto.Text != txtPageManual.Text)
                txtPageManual.Text = txtPageAuto.Text;
        }
        private void menuItemCopyJson_Click(object sender, EventArgs e)
        {
            try
            {
                if (mFilms == null) return;
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    FilmInformation film = (FilmInformation)s[0].Tag;
                    if (film == null) return;
                    Clipboard.SetDataObject(film.ToString(), true);
                    MessageBox.Show("复制成功！", "复制", MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void saveConfig()
        {
            try
            {
                mConfig.Delay = (int)numDelay.Value;
                mConfig.Mode = (byte)(rbStandard.Checked ? 0 : rbSimple.Checked ? 1 : 2);
                mConfig.Score = (double)numScore.Value;
                mConfig.ExpirationTime = (ushort)numExpirationTime.Value;
                mConfig.Save(Application.StartupPath + "JavDB.Polling.json");
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void menuItemIndex_Click(object sender, EventArgs e)
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
        private void btnLoadActorUrl_Click(object sender, EventArgs e)
        {
            loadPage(txtURL2.Text + txtPageAuto.Text, 1, true);
        }
        private void loadPage(string url, int page = 1, bool auto = false)
        {
            try
            {
                saveConfig();
                listInfo.Items.Clear();
                mFilms = new List<FilmInformation>();
                progressBar1.Value = 0;
                Application.DoEvents();
                if (url.IndexOf("page=") >= 0)
                    throw new UriFormatException("地址参数不能包括page.");
                List<FilmInformation> films = new List<FilmInformation>();
                List<string> cate = new List<string>();
                new Thread(() =>
                {
                    try
                    {
                        mPolling = true;
                        this.Invoke(new startPollingDelegate(startPolling), true);
                        int npage = page;
                        do
                        {
                            if (mPolling == false) break;
                            string purl = url;
                            if (purl.IndexOf("?") >= 0)
                            {
                                purl += "&";
                            }
                            else
                            {
                                purl += "?";
                            }
                            purl += $"page={npage}";
                            this.Invoke(new changeStatusDelegate(changeStatus), $"正在加载第 {npage} 页");
                            if (mGrap.GrabPage($"{purl}", out List<FilmInformation> pageFilms))
                            {
                                for (int i = 0; i < pageFilms.Count; i++)
                                {
                                    if (mPolling == false) break;
                                    this.Invoke(new addItemDelegate(addItem), films.Count + i, "等待中", pageFilms[i]);
                                    string file = Path.Combine(mConfig.CachePath, pageFilms[i].SeriesNumber!, pageFilms[i].UID!, pageFilms[i].UID + ".json");
                                    cate.AddRange(pageFilms[i].Category);
                                    if (!File.Exists(file))
                                    {
                                        MakeSureDirectoryPathExists(file);
                                        File.WriteAllText(file, pageFilms[i].ToString());
                                    }
                                    double ww = ((double)i + 1) / pageFilms.Count * 100;
                                    this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)ww);
                                }
                                films.AddRange(pageFilms);
                                npage++;
                            }
                            else
                                break;
                        } while (auto);
                        if (films.Count == 0)
                        {
                            return;
                        }
                        //films.Sort();
                        mFilms = films;
                        this.Invoke(new startPollingDelegate(startPolling), true);
                        for (int i = 0; i < mFilms.Count; i++)
                        {
                            if (mPolling == false) break;
                            this.Invoke(new updateItemDelegate(updateItem), i, "抓取中", mFilms[i]);
                            FilmInformation? film = mFilms[i];
                            try
                            {
                                if (grabFilm(ref film))
                                {
                                    mFilms[i] = film;
                                    cate.AddRange(film.Category);
                                    this.Invoke(new updateItemDelegate(updateItem), i, "抓取成功", film);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.Invoke(new updateItemDelegate(updateItem), i, "抓取失败", new FilmInformation() { Title = ex.Message });
                            }
                            this.Invoke(new progressValueChangeDelegate(progressValueChange), (int)((double)(i + 1) / mFilms.Count * 100));
                        }
                    }
                    catch { }
                    finally
                    {
                        this.Invoke(new startPollingDelegate(startPolling), false);
                        mCategory = CategoryEntity.Parse(cate);
                        initFilter();
                        mPolling = false;
                    }
                })
                { IsBackground = true }.Start();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private bool grabFilm(ref FilmInformation film)
        {
            if (film == null) throw new NullReferenceException("Null of film");
            if (film.UID.IsNullOrEmpty()) throw new NullReferenceException("Null of film.UID");

            string file = Path.Combine(mConfig.CachePath, film.UID!.Split("-")[0], film.UID, film.UID + ".json");
            FilmInformation? item;
            if (File.Exists(file))
            {
                StreamReader sr = File.OpenText(file);
                string cache = sr.ReadToEnd();
                sr.Close();
                item = FilmInformation.Convert(cache);
                if (item != null)
                {
                    DateTime dateTime;
                    DateTime.TryParse(item.GrabTime, out dateTime);
                    if (rbStandard.Checked)
                    {
                        if ((item.Actor.Count == 0 && item.Category.Count == 0) || (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds))
                        {
                            if (mConfig.Delay > 0)
                                Thread.Sleep(mConfig.Delay);
                            mGrap.GrabDetail(ref item);
                        }
                    }
                    else if (rbFliterScore.Checked)
                    {
                        if (!film.Index.IsNullOrEmpty())
                        {
                            item.Score = film.Score;
                        }
                        if (double.Parse(item.Score) >= (double)numScore.Value)
                        {
                            if ((item.Actor.Count == 0 && item.Category.Count == 0) || (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds))
                            {
                                if (mConfig.Delay > 0)
                                    Thread.Sleep(mConfig.Delay);
                                mGrap.GrabDetail(ref item);
                            }
                        }
                    }
                    //else if (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds)
                    //{
                    //    if (rbStandard.Checked)
                    //    {
                    //        if (mConfig.Delay > 0)
                    //            Thread.Sleep(mConfig.Delay);
                    //        item = mGrap.Grab(film.UID!, rbSimple.Checked);
                    //    }
                    //    else
                    //    {
                    //        if (!film.Index.IsNullOrEmpty())
                    //        {
                    //            item.Score = film.Score;
                    //            item.GrabTime = film.GrabTime;
                    //        }
                    //        else
                    //        {
                    //            if (mConfig.Delay > 0)
                    //                Thread.Sleep(mConfig.Delay);
                    //            item = mGrap.Grab(film.UID!, rbSimple.Checked);
                    //        }
                    //    }
                    //}
                }
                else if (film.Index.IsNullOrEmpty())
                {
                    if (mConfig.Delay > 0)
                        Thread.Sleep(mConfig.Delay);
                    if (rbFliterScore.Checked)
                    {
                        item = mGrap.Grab(film.UID, (double)numScore.Value);
                    }
                    else
                    {
                        item = mGrap.Grab(film.UID, rbSimple.Checked);
                    }
                }
                else
                {
                    item = film;
                }
            }
            else
            {
                if (mConfig.Delay > 0)
                    Thread.Sleep(mConfig.Delay);
                if (rbFliterScore.Checked)
                {
                    item = mGrap.Grab(film.UID, (double)numScore.Value);
                }
                else
                {
                    item = mGrap.Grab(film.UID, rbSimple.Checked);
                }
            }
            if (item != null && item.UID == film.UID)
            {
                string path = Path.Combine(mConfig.CachePath, item.SeriesNumber!, item.UID);
                MakeSureDirectoryPathExists(file);
                File.WriteAllText(file, item.ToString());
                if (item.Magnet != null) File.WriteAllText(Path.Combine(path, item.UID + ".html"), item.Magnet);
                film = item;
                return true;
            }
            else
            {
                throw new FileNotFoundException($"{film.UID} NOT FOUND");
            }
        }

        private void menuItemSeriesNumber_Click(object sender, EventArgs e)
        {
            try
            {
                if (mFilms == null) return;
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    FilmInformation film = (FilmInformation)s[0].Tag;
                    if (film == null) return;
                    //https://javdb522.com/video_codes/IDBD
                    tabControl1.SelectedIndex = 1;
                    txtPageManual.Text = "/video_codes/" + film.SeriesNumber;
                    loadPage($"{txtURL.Text}{txtPageManual.Text}");
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
    internal class CategoryEntity : IComparable<CategoryEntity>
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public override string ToString()
        {
            return $"{Name}({Count})";
        }
        public static List<CategoryEntity> Parse(List<string> cate)
        {
            if (cate == null) throw new ArgumentNullException(nameof(cate));
            List<CategoryEntity> categoryEntities = new List<CategoryEntity>();
            cate.Sort();
            string last = string.Empty;
            for (int i = 0; i < cate.Count; i++)
            {
                if (i == 0)
                {
                    categoryEntities.Add(new CategoryEntity() { Name = cate[i], Count = 1 });
                    last = cate[i];
                }
                else
                {
                    if (last == cate[i])
                    {
                        categoryEntities[categoryEntities.Count - 1].Count++;
                    }
                    else
                    {
                        categoryEntities.Add(new CategoryEntity() { Name = cate[i], Count = 1 });
                        last = cate[i];
                    }
                }
            }
            categoryEntities.Sort();
            return categoryEntities;
        }

        public int CompareTo(CategoryEntity? other)
        {
            if (other == null) return -1;
            if (this.Count > other.Count)
            {
                return -1;
            }
            else if (this.Count < other.Count)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}