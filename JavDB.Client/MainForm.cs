using JavDB.Film;
using JavDB.Film.Common;
using Synology.VideoStation.Meta;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;
using static System.Windows.Forms.ListView;
using System;
using System.Security.Cryptography;
using System.Text.Json;
using Accessibility;

namespace JavDB.Client
{
    public partial class MainForm : Form
    {
        private Grappler mGrap;
        private FilmInformation? film;
        private Config mConfig;
        private string? m_Uid = null;
        /// <summary>
        /// 确保文件路径已存在
        /// </summary>
        /// <param name="dirpath"></param>
        /// <returns></returns>
        [DllImport("dbghelp.dll", EntryPoint = "MakeSureDirectoryPathExists", CallingConvention = CallingConvention.StdCall)]
        public static extern long MakeSureDirectoryPathExists(string dirpath);
        public MainForm(string[] args)
        {
            InitializeComponent();
            try
            {
                mGrap = new Grappler(Grappler.ReadFile(Application.StartupPath + "JavDB.Film.json"));
                Config? config = JsonSerializer.Deserialize<Config>(Grappler.ReadFile(Application.StartupPath + "JavDB.Client.json"), Grappler.SerializerOptions);
                if (config != null)
                {
                    mConfig = config;
                }
                else
                {
                    throw new Exception("加载配置文件失败");
                }
                //mGrap.GrabActor("/actors/M4Q7", out List<FilmInformation?> film1,1);
                //film1.Sort();
                //Debug.WriteLine(film1.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mConfig = new Config();
                mGrap = new Grappler("{}");
                mConfig.Save(Application.StartupPath + "JavDB.Client.json");
            }
            try
            {
                picCover.LoadCompleted += (_, _) =>
                {
                    if (film == null) return;
                    string path = Path.Combine(mConfig!.CachePath, film.SeriesNumber!, film.UID!, "Cover.jpg");
                    MakeSureDirectoryPathExists(path);
                    picCover.Image?.Save(path);
                };
                picPoster.LoadCompleted += (_, _) =>
                {
                    if (film == null) return;
                    string path = Path.Combine(mConfig!.CachePath, film.SeriesNumber!, film.UID!, "Poster.jpg");
                    MakeSureDirectoryPathExists(path);
                    picPoster.Image?.Save(path);
                };
                string cache = Path.Combine(Path.GetTempPath(), "JavDB");
                MakeSureDirectoryPathExists(cache);
                var env = CoreWebView2Environment.CreateAsync(userDataFolder: cache).Result;
                webView21.EnsureCoreWebView2Async(env);
                webView22.EnsureCoreWebView2Async(env);
                if (args.Length > 0)
                {
                    m_Uid = args[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (m_Uid.IsNullOrEmpty()) return;
            txtUID.Text = m_Uid!.ToUpper();
            btnGrab_Click(null, null);
        }
        private void btnGrab_Click(object sender, EventArgs e)
        {
            try
            {
                picCover.Image = null;
                picPoster.Image = null;
                listInfo.Items.Clear();
                btnGrab.Text = "正在抓取";
                btnGrab.Enabled = false;
                btnOutputVsMeta.Enabled = false;
                btnCopy.Enabled = false;
                Application.DoEvents();
                string grabUID = txtUID.Text.ToUpper();
                grabUID = grabUID.Replace(" ", "-").Replace("-CH", "").Replace("-UC", "").Replace("-U", "").Replace("-C", "");

                if (chbCacheFirst.Checked && File.Exists(Path.Combine(mConfig.CachePath, grabUID.Split('-')[0], grabUID, grabUID + ".json")))
                {
                    film = FilmInformation.Convert(Grappler.ReadFile(Path.Combine(mConfig.CachePath, grabUID.Split('-')[0], grabUID, grabUID + ".json")));
                    if (film == null)
                    {
                        film = new FilmInformation();
                        film.GrabTime = "1971-01-01 00:00:00";
                    }
                    DateTime dateTime;
                    DateTime.TryParse(film.GrabTime, out dateTime);
                    if (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds)
                    {
                        film = mGrap.Grab(grabUID, false, false);
                        string path = Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID);
                        File.WriteAllText(Path.Combine(path, film.UID + ".json"), film.ToString());
                        if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                    }
                    else if (film.Actor.Count == 0 && film.Category.Count == 0)
                    {
                        mGrap.GrabDetail(ref film);
                        string path = Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID);
                        File.WriteAllText(Path.Combine(path, film.UID + ".json"), film.ToString());
                        if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                    }
                }
                else
                {
                    film = mGrap.Grab(grabUID, false, false);
                    string path = Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID);
                    MakeSureDirectoryPathExists(path + "\\");
                    File.WriteAllText(Path.Combine(path, film.UID + ".json"), film.ToString());
                    if (film.Magnet != null) File.WriteAllText(Path.Combine(path, film.UID + ".html"), film.Magnet);
                }
                if (film != null)
                {
                    if (File.Exists(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "Cover.jpg")))
                    {
                        using FileStream file = File.Open(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "Cover.jpg"), FileMode.Open);
                        picCover.Image = new Bitmap(file);
                    }
                    else
                    {
                        if (film.Cover != null)
                        {
                            picCover.LoadAsync(film.Cover);
                        }
                    }
                    if (File.Exists(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "Poster.jpg")))
                    {
                        using FileStream file = File.Open(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "Poster.jpg"), FileMode.Open);
                        picPoster.Image = new Bitmap(file);
                    }
                    else
                    {
                        if (film.Poster != null)
                        {
                            picPoster.LoadAsync(film.Poster);
                        }
                    }
                    FillBasicInformation(film);
                    FillDetailInformation(film);
                    btnOutputVsMeta.Enabled = true;
                    btnCopy.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            btnGrab.Text = "抓取";
            btnGrab.Enabled = true;
        }
        private void btnOutputVsMeta_Click(object sender, EventArgs e)
        {
            if (film != null)
            {
                VSMetaFile.Output(Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!, txtUID.Text + ".mp4.vsmeta"), film);
            }
        }
        private void FillBasicInformation(FilmInformation film)
        {
            ListViewItem item = new ListViewItem("影片番号");
            item.SubItems.Add(film.UID);
            listInfo.Items.Add(item);
            item = new ListViewItem("主题");
            item.SubItems.Add(film.Title);
            listInfo.Items.Add(item);
            item = new ListViewItem("发行日期");
            item.SubItems.Add(film.Date);
            listInfo.Items.Add(item);
            item = new ListViewItem("主页地址");
            item.SubItems.Add(film.Index);
            listInfo.Items.Add(item);
            item = new ListViewItem("封面");
            item.SubItems.Add(film.Cover);
            listInfo.Items.Add(item);
        }
        private void FillDetailInformation(FilmInformation film)
        {
            ListViewItem item = new ListViewItem("海报");
            item.SubItems.Add(film.Poster);
            listInfo.Items.Add(item);
            item = new ListViewItem("预览");
            item.SubItems.Add(film.PreviewVideo);
            listInfo.Items.Add(item);
            item = new ListViewItem("级别");
            item.SubItems.Add(film.Level);
            listInfo.Items.Add(item);
            item = new ListViewItem("时长");
            item.SubItems.Add(film.Durations);
            listInfo.Items.Add(item);
            item = new ListViewItem("导演");
            item.SubItems.Add(film.Director);
            item = new ListViewItem("片商");
            item.SubItems.Add(film.FilmDistributor);
            listInfo.Items.Add(item);
            item = new ListViewItem("发行");
            item.SubItems.Add(film.Issue);
            listInfo.Items.Add(item);
            item = new ListViewItem("系列");
            item.SubItems.Add(film.Series);
            listInfo.Items.Add(item);
            item = new ListViewItem("评分");
            item.SubItems.Add(film.Score);
            listInfo.Items.Add(item);
            item = new ListViewItem("类别");
            item.SubItems.Add(film.Category.GetString(','));
            listInfo.Items.Add(item);
            foreach (var act in film.Actor)
            {
                item = new ListViewItem("演员");
                item.SubItems.Add(act.Name + (act.Gender == Film.Gender.MALE ? "(男)" : "(女)"));
                item.Tag = mGrap.SRC + act.Url;
                listInfo.Items.Add(item);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {

            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (film != null && film.PreviewVideo != null)
                {
                    webView21.Source = new Uri($"{mConfig.PlayerURL}?url={film.PreviewVideo}&muted=true&poster={film.Poster}");
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (film != null && film.PreviewVideo != null)
                {
                    webView22.Source = new Uri(Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!, film.UID + ".html"));
                }
            }
        }

        private async void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (webView21 != null && webView21.CoreWebView2 != null)
            {
                await webView21.CoreWebView2.Profile.ClearBrowsingDataAsync();
            }
            if (webView22 != null && webView22.CoreWebView2 != null)
            {
                await webView22.CoreWebView2.Profile.ClearBrowsingDataAsync();
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
                    Clipboard.SetDataObject(this.listInfo.Items[nowIndex].SubItems[1].Text, true);
                    MessageBox.Show("复制文本成功！", "复制", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(film!.ToString(), true);
                MessageBox.Show("复制文本成功！", "复制", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    string text = "";
                    if (this.listInfo.Items[nowIndex].SubItems[0].Text == "演员")
                    {
                        text = this.listInfo.Items[nowIndex].Tag.ToString()!;
                    }
                    else if (this.listInfo.Items[nowIndex].SubItems[0].Text == "主页地址")
                    {
                        text = mGrap.SRC + this.listInfo.Items[nowIndex].SubItems[1].Text;
                    }
                    else
                    {
                        text = this.listInfo.Items[nowIndex].SubItems[1].Text;
                    }
                    if (text.IndexOf("http") >= 0)
                    {
                        Process process = new Process();
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(text);
                        process.StartInfo = processStartInfo;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
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
                SelectedListViewItemCollection s = this.listInfo.SelectedItems;
                if (s.Count > 0)
                {
                    int nowIndex = s[0].Index;
                    string text = this.listInfo.Items[nowIndex].SubItems[0].Text;
                    if (text == "主页地址" || text == "封面" || text == "海报" || text == "演员")
                    {
                        menuItemOpen.Enabled = true;
                        e.Cancel = false;
                    }
                    else
                    {
                        menuItemOpen.Enabled = false;
                        e.Cancel = true;
                    }
                }
            }
            catch
            {
            }
        }
    }
    public class VSMetaFile
    {
        public static void Output(string filename, FilmInformation film)
        {
            FileInfo file = new FileInfo(filename);
            MovieInformation movie = new MovieInformation();
            movie.Title = film.UID;
            movie.Title2 = film.UID;
            movie.SubTitle = film.Title;
            movie.Year = film.Date == null ? 1971 : int.Parse(film.Date.Substring(0, 4));
            movie.Date = film.Date;
            movie.Summary = film.Title;
            movie.MetaJson = "{}";
            movie.Actor = new List<string>();
            foreach (FilmActor actor in film.Actor)
            {
                if (actor.Gender == Gender.WOMAN) movie.Actor.Add(actor.Name);
            }
            movie.Director = film.Director;
            movie.Category = film.Category;
            movie.FilmDistributor = film.FilmDistributor;
            movie.Level = film.Level;
            movie.Score = film.Score;
            movie.Cover = file.DirectoryName + "\\Cover.jpg";
            movie.Poster = file.DirectoryName + "\\Poster.jpg";
            MetaFile.WriteToFile(filename, movie);
            //调试输出(“输出VSMETA：” ＋ movie_path ＋ “\” ＋ vsmeta_outfile ＋ “.vsmeta”)
            Process.Start("Explorer", $" /select,\"{filename}\"");
        }
    }
}