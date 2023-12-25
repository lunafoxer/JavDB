using JavDB.Film.Common;
using JavDB.Film.Entity;
using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Text;
using System.Text.Json;
using System.Numerics;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Reflection;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace JavDB.Film
{
    public class Grappler
    {
        private Config? mConfig;
        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = JsonNumberHandling.AllowReadingFromString, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
        public string? SRC => mConfig?.src;
        public Grappler(string configuration, double scoreMultiplier) : this(configuration)
        {
            mConfig!.scoreMultiplier = scoreMultiplier;
        }
        public Grappler(string configuration)
        {
            mConfig = JsonSerializer.Deserialize<Config>(configuration, SerializerOptions);
            if (mConfig == null)
            {
                throw new NullReferenceException(nameof(mConfig));
            }
        }
        public FilmInformation Grab(string uid, bool simple = false, bool perfectMatch = true)
        {
            if (mConfig == null) throw new NullReferenceException(nameof(mConfig));
            if (string.IsNullOrEmpty(uid)) throw new ArgumentNullException("uid");
            GrabSimple(uid, out FilmInformation film, perfectMatch);
            if (!simple)
            {
                GrabDetail(ref film);
            }
            return film;
        }
        public FilmInformation Grab(string uid, double score, bool perfectMatch = true)
        {
            if (mConfig == null) throw new NullReferenceException(nameof(mConfig));
            if (string.IsNullOrEmpty(uid)) throw new ArgumentNullException("uid");
            GrabSimple(uid, out FilmInformation film, perfectMatch);
            if (score > 0D && score <= 10D && !string.IsNullOrEmpty(film.Score))
            {
                if (double.Parse(film.Score) >= score)
                    GrabDetail(ref film);
            }
            return film;
        }
        public void GrabSimple(string uid, out FilmInformation film, bool perfectMatch = true)
        {
            if (mConfig == null) throw new NullReferenceException(nameof(mConfig));
            if (string.IsNullOrEmpty(uid)) throw new ArgumentNullException("uid");
            var web = new HtmlWeb()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.50"
            };
            //创建一个html的解析器
            //使用解析器解析文档
            var parser = web.Load($"{mConfig.src}/search?f=all&q={uid}");
            if (parser == null)
            {
                throw new Exception("请求网页失败");
            }
            //parser.Load("g:\\123.html", Encoding.UTF8);
            var dom = parser.DocumentNode.SelectNodes(mConfig.grab.path);
            if (dom == null || dom.Count == 0)
            {
                throw new Exception("未搜索到影片");
            }
            int index = 1;
            FilmInformation result = new FilmInformation();
            HtmlNode item;
            do
            {
                item = dom[0].SelectSingleNode(mConfig.grab.item.path.Replace("$index", index.ToString()));
                if (item != null)
                {
                    if (ResolvingItem(item, out result))
                    {
                        if (result.UID != null && result.UID == uid)
                        {
                            film = result;
                            return;
                        }
                    }
                }
                else
                    break;
                index++;
            } while (index <= 20);
            item = dom[0].SelectSingleNode(mConfig.grab.item.path.Replace("$index", "1"));
            if (item != null)
            {
                if (ResolvingItem(item, out result))
                {
                    if (!perfectMatch)
                    {
                        film = result;
                    }
                    else
                        throw new Exception("未搜索到影片");
                }
                else
                    throw new Exception("解析影片基本信息失败");
            }
            else
                throw new Exception("没有任何结果");
        }
        public void GrabDetail(ref FilmInformation film)
        {
            if (mConfig == null) throw new NullReferenceException(nameof(mConfig));
            if (film == null) throw new ArgumentNullException(nameof(film));
            var web = new HtmlWeb()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.50"
            };
            var parser = web.Load(SRC + film.Index);
            var item = parser.DocumentNode.SelectSingleNode(mConfig.grab.index.path);
            if (item == null)
                throw new Exception("解析影片详细信息失败");
            if (string.IsNullOrEmpty(film.Poster))
                film.Poster = item.SelectSingleNode(mConfig.grab.index.poster)?.GetAttributeValue("src", "");
            if (string.IsNullOrEmpty(film.Cover))
                film.Cover = film.Poster?.Replace("covers", "thumbs");
            //分析基本信息节点
            var info = item.SelectNodes(mConfig.grab.index.info.path);
            foreach (var it in info)
            {
                try
                {
                    var sp = it.SelectSingleNode(mConfig.grab.index.info.type);
                    if (sp == null) continue;
                    string type = sp.InnerText;
                    switch (type)
                    {
                        case "時長:":
                            film.Durations = it.SelectSingleNode("span")?.InnerText.Middle(null, " 分鍾");
                            break;
                        case "導演:":
                            film.Director = it.SelectSingleNode("span/a")?.InnerText.Trim();
                            break;
                        case "片商:":
                            film.FilmDistributor = it.SelectSingleNode("span/a")?.InnerText.Trim();
                            break;
                        case "發行:":
                            film.Issue = it.SelectSingleNode("span/a")?.InnerText.Trim();
                            break;
                        case "系列:":
                            film.Series = it.SelectSingleNode("span/a")?.InnerText.Trim();
                            break;
                        case "評分:":
                            film.Score = it.SelectSingleNode("span")?.InnerText.Middle("&nbsp;", "分, ");
                            if (!string.IsNullOrEmpty(film.Score))
                                film.Score = Math.Round(double.Parse(film.Score.Trim()) * mConfig.scoreMultiplier, 1).ToString();
                            break;
                        case "類別:":
                            var dt = it.SelectNodes("span/a");
                            film.Category = new List<string>();
                            foreach (var node in dt)
                            {
                                film.Category.Add(node.InnerText.Trim());
                            }
                            break;
                        case "演員:":
                            var da = it.SelectNodes("span");
                            film.Actor = new List<FilmActor>();
                            for (int vi = 0; vi < da[0].SelectNodes("a")?.Count; vi++)
                            {
                                FilmActor act = new FilmActor();
                                string? s = da[0].SelectNodes("strong")[vi]?.InnerText;
                                if (!string.IsNullOrEmpty(s))
                                {
                                    if (s == "♂")
                                        act.Gender = Gender.MALE;
                                    else if (s == "♀")
                                        act.Gender = Gender.WOMAN;
                                }
                                act.Name = da[0].SelectNodes("a")[vi]?.InnerText.Trim();
                                act.Url = da[0].SelectNodes("a")[vi]?.GetAttributeValue("href", "");
                                act.Url = act.Url?.Replace("about:", "");
                                film.Actor.Add(act);
                            }
                            break;
                    }
                }
                catch { }
            }
            //分析预览节点
            film.PreviewVideo = item.SelectSingleNode(mConfig.grab.index.previewVideo)?.GetAttributeValue("src", "");
            if (!string.IsNullOrEmpty(film.PreviewVideo) && film.PreviewVideo?.IndexOf("http") < 0)
            {
                film.PreviewVideo = "https:" + film.PreviewVideo;
            }
            //分析磁力链接节点
            var dma = item.SelectSingleNode(mConfig.grab.index.magnet);
            if (dma != null)
            {
                film.Magnet = baseString.magent_html.Replace("#TAG_MAGENT#", dma.OuterHtml);
                film.Magnet = film.Magnet.Replace(dma.SelectSingleNode(mConfig.grab.index.clearMagnet).OuterHtml, "");
                film.Magnet = film.Magnet.Replace("data-clipboard-text=", baseString.magent_tag + "value= ");
            }
            film.GrabTime = DateTime.Now.ToString();
        }

        public bool GrabPage(string pageUrl, out List<FilmInformation> films, bool simple = true)
        {
            if (mConfig == null) throw new NullReferenceException(nameof(mConfig));
            if (string.IsNullOrEmpty(pageUrl)) throw new ArgumentNullException(nameof(pageUrl));
            var web = new HtmlWeb()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36 Edg/113.0.1774.50"
            };
            films = new List<FilmInformation>();
            //创建一个html的解析器
            //使用解析器解析文档
            Debug.WriteLine(pageUrl);
            var parser = web.Load(pageUrl);
            //var parser = web.Load(@"e:\t.html");
            if (parser == null)
            {
                throw new Exception("请求网页失败");
            }
            var dom = parser.DocumentNode.SelectNodes(mConfig.grab.path);
            if (dom == null || dom.Count == 0)
            {
                Debug.WriteLine("暂无结果");
                return false;
            }
            int index = 1;
            do
            {
                var item = dom[0].SelectSingleNode(mConfig.grab.item.path.Replace("$index", index.ToString()));
                if (item == null) break;
                if (ResolvingItem(item, out FilmInformation? film))
                {
                    if (film != null)
                    {
                        if (!simple)
                        {
                            GrabDetail(ref film);
                        }
                        films.Add(film);
                    }
                }
                index++;
            } while (index <= 100);
            return true;
        }
        private bool ResolvingItem(HtmlNode item, out FilmInformation film)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            film = new FilmInformation();
            film.SRC = mConfig!.src;
            film.Index = item.GetAttributeValue("href", "");
            if (string.IsNullOrEmpty(film.Index)) return false;
            film.Level = "R-18";
            film.GrabTime = DateTime.Now.ToString();
            film.Title = item.GetAttributeValue(mConfig.grab.item.title, "");
            film.Poster = item.SelectSingleNode(mConfig.grab.item.poster)?.GetAttributeValue("src", "");
            film.Cover = film.Poster?.Replace("covers", "thumbs");
            film.UID = item.SelectSingleNode(mConfig.grab.item.uid)?.InnerText;
            film.Score = item.SelectSingleNode(mConfig.grab.item.score)?.InnerText.Middle("&nbsp;", "分, ");
            if (!string.IsNullOrEmpty(film.Score))
                film.Score = Math.Round(double.Parse(film.Score.Trim()) * mConfig.scoreMultiplier, 1).ToString();
            film.Date = item.SelectSingleNode(mConfig.grab.item.date)?.InnerText.Replace(((char)10).ToString(), "").Trim();
            return true;
        }
        public static string ReadFile(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException($"无法加载文件，文件不存在：\n{filename}");
            using StreamReader sr = File.OpenText(filename);
            string cache = sr.ReadToEnd();
            sr.Close();
            return cache;
        }
    }
}