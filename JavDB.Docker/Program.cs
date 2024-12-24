using JavDB.Extentions.Nfo;
using JavDB.Film;
using JavDB.Film.Common;
using Synology.VideoStation.Meta;
using System.Runtime;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using System.Net.Http.Headers;
using System.IO;

namespace JavDB.Docker
{
    internal class Program
    {
        private static Config mConfig = new Config();
        static void Main(string[] args)
        {
            printi("正在启动服务并加载配置参数...");
            Grappler grappler = new Grappler(Grappler.ReadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "JavDB.Film.json")));
            string? interval = Environment.GetEnvironmentVariable("INTERVAL");
            int Interval = 0;
            if (string.IsNullOrEmpty(interval))
            {
                Interval = 300000;
            }
            else
            {
                if (int.TryParse(interval, out int intw))
                {
                    Interval = intw;
                }
                else
                {
                    Interval = 300000;
                }
            }
            printi($"检测目录：{mConfig.ListenPath.GetString(';')}[配置文件：{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "listen.txt")}]");
            printi($"检测文件类型：{mConfig.FileExtensions.GetString(';')}[配置文件：{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "fileEx.txt")}]");
            if (grappler.proxy != null && grappler.proxy.enabled)
            {
                printi($"代理服务器：已启用，[{grappler.proxy.ToString()}]");
            }
            while (true)
            {
                try
                {
                    printi("开始批处理。");
                    indexDir(grappler, mConfig.ListenPath.ToArray());
                }
                catch (Exception e)
                {
                    e.Error();
                }
                printi($"批处理执行完毕，下次执行时间：{DateTime.Now.AddMilliseconds(Interval)}。");
                Thread.Sleep(Interval);
            }
        }
        static void indexDir(Grappler grappler, string[] path)
        {
            List<string> files = new List<string>();

            foreach (string dir in path)
            {
                try
                {
                    foreach (string ext in mConfig.FileExtensions)
                    {
                        var lists = Directory.EnumerateFiles(dir, ext, SearchOption.AllDirectories);
                        files.AddRange(lists);
                    }
                }
                catch (Exception e1)
                {
                    e1.Error();
                }
            }
            foreach (var file in files)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string uid = Path.GetFileNameWithoutExtension(fileInfo.Name).ToUpper();
                    string vsmeta = fileInfo.FullName + ".vsmeta";
                    string nfo = Path.Combine(fileInfo.Directory?.FullName, "movie.nfo");
                    if (!File.Exists(vsmeta) || !File.Exists(nfo))
                    {
                        printi($"正在处理：{fileInfo.FullName}");
                        string grabUID = uid.Replace(" ", "-").Replace("-CH", "").Replace("-UC", "").Replace("-U", "").Replace("-C", "");
                        var film = grab(grappler, grabUID, fileInfo.Directory.FullName);
                        if (film != null)
                        {
                            VSMetaFile.Output(fileInfo.FullName + ".vsmeta", film);
                            NfoFile.Output(nfo, film);
                            printi($"文件 {fileInfo.FullName} 处理完毕。");
                        }
                        else
                        {
                            printi($"文件 {fileInfo.FullName} 处理失败，请检查日志以获取错误信息。");
                        }
                    }
                }
                catch (Exception e2)
                {
                    e2.Error();
                }
            }

        }
        private static FilmInformation? grab(Grappler grappler, string grabUID, string dirPath)
        {
            try
            {
                string jsonFile = Path.Combine(dirPath, grabUID + ".json");
                FilmInformation? film;
                if (File.Exists(jsonFile))
                {
                    printi($"加载缓存文件：{jsonFile}");
                    film = FilmInformation.Convert(Grappler.ReadFile(jsonFile));
                    if (film == null)
                    {
                        film = new FilmInformation();
                        film.GrabTime = "1971-01-01 00:00:00";
                    }
                    DateTime dateTime;
                    DateTime.TryParse(film.GrabTime, out dateTime);
                    if (DateTime.Now.GetUnixTimestamp() - dateTime.GetUnixTimestamp() > mConfig.ExpirationSeconds)
                    {
                        printi($"缓存文件过期，正在重新抓取...");
                        film = grappler.Grab(grabUID, false, false);
                        File.WriteAllText(jsonFile, film.ToString());
                        if (film.Magnet != null) File.WriteAllText(Path.Combine(dirPath, film.UID + ".html"), film.Magnet);
                        printi($"重新抓取完成。");
                    }
                    else if (film.Actor.Count == 0 && film.Category.Count == 0)
                    {
                        printi($"缓存文件信息缺失，正在重新抓取...");
                        grappler.GrabDetail(ref film);
                        File.WriteAllText(jsonFile, film.ToString());
                        if (film.Magnet != null) File.WriteAllText(Path.Combine(dirPath, film.UID + ".html"), film.Magnet);
                        printi($"重新抓取完成。");
                    }
                }
                else
                {
                    printi($"正在抓取...");
                    film = grappler.Grab(grabUID, false, false);
                    File.WriteAllText(jsonFile, film.ToString());
                    if (film.Magnet != null) File.WriteAllText(Path.Combine(dirPath, film.UID + ".html"), film.Magnet);
                    printi($"抓取完成。");
                }
                if (film != null)
                {
                    string poster = Path.Combine(dirPath, "poster.jpg");
                    if (!File.Exists(poster))
                    {
                        if (film.Poster != null)
                        {
                            try
                            {
                                printi($"正在下载封面图片。");
                                HttpGet(out byte[] data, film.Poster);
                                using (BinaryWriter bw = new BinaryWriter(File.Open(poster, FileMode.Create)))
                                {
                                    bw.Write(data);
                                    printi($"下载完成。");
                                }
                            }
                            catch (Exception e)
                            {
                                e.Error();
                                printi($"下载失败，请检查日志以获取错误信息。");
                            }
                        }
                    }
                    string backdrop = Path.Combine(dirPath, "backdrop.jpg");
                    if (!File.Exists(backdrop))
                    {
                        if (film.Backdrop != null)
                        {
                            try
                            {
                                printi($"正在下载背景墙图片。");
                                HttpGet(out byte[] data, film.Backdrop);
                                using (BinaryWriter bw = new BinaryWriter(File.Open(backdrop, FileMode.Create)))
                                {
                                    bw.Write(data);
                                    printi($"下载完成。");
                                }
                            }
                            catch (Exception e)
                            {
                                e.Error();
                                printi($"下载失败，请检查日志以获取错误信息。");
                            }
                        }
                    }
                }
                return film;
            }
            catch (Exception ex)
            {
                ex.Error();
                return null;
            }
        }
        static void print(string tag, string text)
        {
            Console.WriteLine($"[{DateTime.Now}][{tag}]:{text}");
        }
        static void printi(string text)
        {
            print("INFO", text);
        }
        static void printw(string text)
        {
            print("WRAN", text);
        }
        static void printe(string text)
        {
            print("ERROR", text);
        }
        ///<summary>
        /// 同步get请求
        ///</summary>
        ///<param name="url">链接地址</param>
        ///<param name="formData">写在header中的键值对</param>
        ///<returns></returns>
        public static int HttpGet(out byte[] result, string url, List<KeyValuePair<string, string>>? formData = null)
        {
            HttpClient httpclient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            if (formData != null)
            {
                HttpContent content = new FormUrlEncodedContent(formData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-ww-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                for (int i = 0; i < formData.Count; i++)
                {
                    content.Headers.Add(formData[i].Key, formData[i].Value);
                    //request.Headers.Add(formData[i].Key, formData[i].Value);
                }
                request.Content = content;
            }
            var res = httpclient.SendAsync(request);
            res.Wait();
            var resp = res.Result;
            Task<byte[]> temp = resp.Content.ReadAsByteArrayAsync();
            temp.Wait();
            result = temp.Result;
            return 0;
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
            using BinaryReader fs = new(File.Open(Path.Combine(file.DirectoryName, "poster.jpg"), FileMode.Open));
            movie.Images.Episode = fs.ReadBytes((int)fs.BaseStream.Length);
            using BinaryReader fsb = new(File.Open(Path.Combine(file.DirectoryName, "backdrop.jpg"), FileMode.Open));
            movie.Images.Backdrop = fsb.ReadBytes((int)fsb.BaseStream.Length);
            movie.Locked = true;
            MetaFileStream.WriteToFile(filename, movie, META_HEAD.TAG_TYPE_MOVIE);
            //调试输出(“输出VSMETA：” ＋ movie_path ＋ “\” ＋ vsmeta_outfile ＋ “.vsmeta”)
        }
        public static void Output(string filename, movie nfoMovie)
        {
            FileInfo file = new FileInfo(filename);
            MovieInformation movie = new MovieInformation();
            movie.Title = nfoMovie.title;
            movie.Title2 = nfoMovie.title;
            movie.SubTitle = nfoMovie.originaltitle;
            movie.Year = (int)(nfoMovie.year == null ? 1971 : nfoMovie.year);
            movie.Date = nfoMovie.releasedate?.ToString("yyyy-MM-dd");
            movie.Summary = nfoMovie.plot;
            movie.MetaJson = "{}";
            movie.Actor = new List<string>();
            if (nfoMovie.actor != null)
            {
                foreach (var actor in nfoMovie.actor)
                {
                    if (actor.name != null && actor.type?.ToLower() == "actor") movie.Actor.Add(actor.name);
                }
            }
            movie.Director = nfoMovie.director;
            movie.Category = nfoMovie.genre;
            movie.FilmDistributor = nfoMovie.studio?.GetString(',');
            movie.Level = nfoMovie.mpaa;
            movie.Score = nfoMovie.rating;

            if (movie.Images == null) movie.Images = new ImageInfo();
            using BinaryReader fs = new(File.Open(Path.Combine(file.DirectoryName, "poster.jpg"), FileMode.Open));
            movie.Images.Episode = fs.ReadBytes((int)fs.BaseStream.Length);
            using BinaryReader fsb = new(File.Open(Path.Combine(file.DirectoryName, "backdrop.jpg"), FileMode.Open));
            movie.Images.Backdrop = fsb.ReadBytes((int)fsb.BaseStream.Length);
            movie.Locked = true;
            MetaFileStream.WriteToFile(filename, movie, META_HEAD.TAG_TYPE_MOVIE);
            //调试输出(“输出VSMETA：” ＋ movie_path ＋ “\” ＋ vsmeta_outfile ＋ “.vsmeta”)

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
            movie movie = new movie();
            movie.plot = film.Title;
            movie.lockdata = false;
            movie.dateadded = DateTime.Parse(film.GrabTime);
            movie.title = film.UID;
            movie.originaltitle = film.UID;
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

            movie.actor = new List<actor>();
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
                    movie.actor.Add(act);
                }
            }
            movie.Write(filename, movie);
            //MetaFileStream.WriteToFile(filename, movie, META_HEAD.TAG_TYPE_MOVIE);
            //调试输出(“输出VSMETA：” ＋ movie_path ＋ “\” ＋ vsmeta_outfile ＋ “.vsmeta”)
            //Process.Start("Explorer", $" /select,\"{filename}\"");
        }
    }
}
