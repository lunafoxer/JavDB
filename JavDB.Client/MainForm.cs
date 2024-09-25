using JavDB.Film;
using JavDB.Film.Common;
using JavDB.Extentions.Nfo;
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
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using static JavDB.Client.VSMetaFile;
using Microsoft.VisualBasic.Devices;
using System.Xml;
using System.Text;

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
                picPoster.LoadCompleted += (_, _) =>
                {
                    if (film == null) return;
                    string path = Path.Combine(mConfig!.CachePath, film.SeriesNumber!, film.UID!, "poster.jpg");
                    MakeSureDirectoryPathExists(path);
                    picPoster.Image?.Save(path);
                };
                picBackdrop.LoadCompleted += (_, _) =>
                {
                    if (film == null) return;
                    string path = Path.Combine(mConfig!.CachePath, film.SeriesNumber!, film.UID!, "backdrop.jpg");
                    MakeSureDirectoryPathExists(path);
                    picBackdrop.Image?.Save(path);
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
                picPoster.Image = null;
                picBackdrop.Image = null;
                listInfo.Items.Clear();
                btnGrab.Text = "正在抓取";
                btnGrab.Enabled = false;
                btnOutputMetadata.Enabled = false;
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
                    if (File.Exists(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "poster.jpg")))
                    {
                        using FileStream file = File.Open(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "poster.jpg"), FileMode.Open);
                        picPoster.Image = new Bitmap(file);
                    }
                    else
                    {
                        if (film.Poster != null)
                        {
                            picPoster.LoadAsync(film.Poster);
                        }
                    }
                    if (File.Exists(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "backdrop.jpg")))
                    {
                        using FileStream file = File.Open(Path.Combine(mConfig.CachePath, film.SeriesNumber, film.UID, "backdrop.jpg"), FileMode.Open);
                        picBackdrop.Image = new Bitmap(file);
                    }
                    else
                    {
                        if (film.Backdrop != null)
                        {
                            picBackdrop.LoadAsync(film.Backdrop);
                        }
                    }
                    FillBasicInformation(film);
                    FillDetailInformation(film);
                    btnOutputMetadata.Enabled = true;
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
                NfoFile.Output(Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!, "movie.nfo"), film);
                VSMetaFile.Output(Path.Combine(mConfig.CachePath, film.SeriesNumber!, film.UID!, txtUID.Text + ".mp4.vsmeta"), film);
            }
        }
        private void FillBasicInformation(FilmInformation film)
        {
            addItem("影片番号", film.UID);
            addItem("主题", film.Title);
            addItem("发行日期", film.Date);
            addItem("主页地址", film.Index);
            addItem("封面", film.Poster);
        }
        private void FillDetailInformation(FilmInformation film)
        {
            addItem("海报", film.Backdrop);
            addItem("预览", film.PreviewVideo);
            addItem("级别", film.Level);
            addItem("时长", film.Durations);
            addItem("导演", film.Director);
            addItem("片商", film.FilmDistributor);
            addItem("发行", film.Issue);
            addItem("系列", film.Series);
            addItem("评分", film.Score);
            addItem("类别", film.Category.GetString(','));
            foreach (var act in film.Actor)
            {
                addItem("演员", act.Name + (act.Gender == Film.Gender.MALE ? "(男)" : "(女)"), mGrap.SRC + act.Url);
            }
        }
        private void addItem(string itemName, string? itemValue, string? tag = null)
        {
            if (itemName.IsNullOrEmpty()) return;
            if (itemValue.IsNullOrEmpty()) return;
            ListViewItem item = new ListViewItem(itemName);
            item.SubItems.Add(itemValue);
            if (!tag.IsNullOrEmpty()) item.Tag = tag;
            listInfo.Items.Add(item);
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
                    webView21.Source = new Uri($"{mConfig.PlayerURL}?url={film.PreviewVideo}&muted=true&poster={film.Backdrop}");
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

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (txtUID.Text.Length > 0)
            {
                txtUID.SelectAll();
            }
            txtUID.Focus();
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

            if (movie.Images == null) movie.Images = new ImageInfo();
            using BinaryReader fs = new(File.Open(file.DirectoryName + "\\poster.jpg", FileMode.Open));
            movie.Images.Episode = fs.ReadBytes((int)fs.BaseStream.Length);
            using BinaryReader fsb = new(File.Open(file.DirectoryName + "\\backdrop.jpg", FileMode.Open));
            movie.Images.Backdrop = fsb.ReadBytes((int)fsb.BaseStream.Length);
            movie.Locked = true;
            MetaFileStream.WriteToFile(filename, movie, META_HEAD.TAG_TYPE_MOVIE);
            //调试输出(“输出VSMETA：” ＋ movie_path ＋ “\” ＋ vsmeta_outfile ＋ “.vsmeta”)
            Process.Start("Explorer", $" /select,\"{filename}\"");
        }

        internal class MovieInformation : IMovieInformation
        {
            public string? Title { get; set; }
            public string? Title2 { get; set; }
            public string? SubTitle { get; set; }
            public int Year { get; set; }
            public string? Date { get; set; }
            public string? Level { get; set; }
            public string? Director { get; set; }
            public string? FilmDistributor { get; set; }
            public string? Summary { get; set; }
            public string? Score { get; set; }
            public string? MetaJson { get; set; }
            public List<string> Category { get; set; } = new List<string>();
            public List<string> Actor { get; set; } = new List<string>();
            public bool Locked { get; set; }
            public ImageInfo? Images { get; set; }
            public TvshowInfo? Tvshow { get; set; }
            public int Timestamp { get; set; }

            public MovieInformation()
            {
                Images = new ImageInfo();
                Tvshow = new TvshowInfo();
            }
            /// <summary>
            /// json
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
            }
            public static MovieInformation? Convert(string json)
            {
                return JsonSerializer.Deserialize<MovieInformation>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
            }
        }
    }
    public class NfoFile
    {
        public static void Output(string filename, FilmInformation film)
        {
            FileInfo file = new FileInfo(filename);
            movie movie = new movie();
            movie.plot = film.Title;
            movie.lockdata = false;
            movie.dateadded = DateTime.Parse(film.GrabTime);
            movie.title = film.UID;
            movie.originaltitle = film.Title;
            movie.director = film.Director;
            // movie.writer = null;
            movie.trailer = new List<string> { film.PreviewVideo };
            movie.rating = film.Score;
            movie.year = (ushort?)(film.Date == null ? 1971 : int.Parse(film.Date.Substring(0, 4)));
            movie.premiered = DateTime.Parse(film.Date);
            movie.releasedate = movie.premiered;
            movie.mpaa = film.Level;
            //movie.runtime =film.Durations;
            movie.art = new art() { fanart = "backdrop.jpg", poster = "poster.jpg" };
            movie.genre = new List<string>();
            foreach (var s in film.Category)
            {
                movie.genre.Add(s);
            }
            movie.studio = new List<string> { film.FilmDistributor };

            movie.actors = new List<actor>();
            short index = 0;
            foreach (FilmActor actor in film.Actor)
            {
                if (actor.Gender == Gender.WOMAN)
                {
                    actor act = new actor();
                    act.name = actor.Name;
                    act.role = "女主角";
                    act.type = "Actor";
                    act.sortorder = index;
                    index++;
                    movie.actors.Add(act);
                }
            }
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(movie.ToString());
                fs.SetLength(0);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
            //MetaFileStream.WriteToFile(filename, movie, META_HEAD.TAG_TYPE_MOVIE);
            //调试输出(“输出VSMETA：” ＋ movie_path ＋ “\” ＋ vsmeta_outfile ＋ “.vsmeta”)
            //Process.Start("Explorer", $" /select,\"{filename}\"");
        }
    }
}