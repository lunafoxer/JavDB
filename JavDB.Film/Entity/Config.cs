using System.Text.Json;

namespace JavDB.Film.Entity
{
    internal static class baseString
    {
        public const string magent_html = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">\r\n<html>\r\n\r\n<head>\r\n  <meta name=\"theme\" content=\"auto\">\r\n  <meta content=\"True\" name=\"HandheldFriendly\">\r\n  <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\r\n  <meta name=\"renderer\" content=\"webkit\">\r\n  <meta name=\"force-rendering\" content=\"webkit\">\r\n  <title>磁力链接</title>\r\n  <link rel=\"stylesheet\" media=\"all\" href=\"#TAG_CSS_SRC#\">\r\n  <script language=\"javascript\">\r\n    function copyToClipboard(obj) {\r\n      var s = obj.value;\r\n      if (window.clipboardData) {\r\n        window.clipboardData.setData('text', s);\r\n      } else {\r\n        (function (s) {\r\n          document.oncopy = function (e) {\r\n            e.clipboardData.setData('text', s);\r\n            e.preventDefault();\r\n            document.oncopy = null;\r\n          }\r\n        })(s);\r\n        document.execCommand('Copy');\r\n      }\r\n    }\r\n  </script>\r\n</head>\r\n\r\n<body scroll=\"auto\">\r\n#TAG_MAGENT#\r\n</body>\r\n\r\n</html>";
        public const string magent_tag = " onclick=\"copyToClipboard(this)\" ";
    }
    internal class Config
    {
        public string src { get; set; } = "https://javdb524.com";
        public double scoreMultiplier { get; set; }
        public Proxy proxy { get; set; } = new Proxy();
        public GrabConfiguration grab { get; set; } = new GrabConfiguration();
        public Config(string src, double scoreMultiplier = 1.85)
        {
            this.src = src;
            this.scoreMultiplier = scoreMultiplier;
        }
        public void SetProxy(string proxyHost, int proxyPort, string userId, string password)
        {
            this.proxy = new Proxy() { enabled = true, host = proxyHost, port = proxyPort, userId = userId, password = password };
        }
        public void SetProxy(Proxy p)
        {
            this.proxy = p;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, Grappler.SerializerOptions);
        }
    }
    internal class GrabConfiguration
    {
        public Basic basic { get; set; } = new Basic();
        public Index index { get; set; } = new Index();
    }
    internal class Basic
    {
        public string path { get; set; } = "/html/body/section/div/div[@class='movie-list h cols-4 vcols-8']";
        public Item item { get; set; } = new Item();
    }
    internal class Item
    {
        public string path { get; set; } = "div[$index]/a[@class='box']";
        public string title { get; set; } = "title";
        public string backdrop { get; set; } = "div/img";
        public string score { get; set; } = "div[@class='score']";
        public string uid { get; set; } = "div[@class='video-title']/strong";
        public string date { get; set; } = "div[@class='meta']";
    }
    internal class Index
    {
        public string path { get; set; } = "/html/body/section/div/div[@class='video-detail']";
        public string css { get; set; } = "/html/head/link[@rel='stylesheet']";
        public string backdrop { get; set; } = "div/div/div/a/img";
        public string previewVideo { get; set; } = "div[2]/div/article/div/div/video[@id='preview-video']/source";
        public string magnet { get; set; } = "div[@data-controller='movie-tab']/div/div[@id='tabs-container']/div[@id='magnets']/article";
        public string clearMagnet { get; set; } = "div/div[@class='top-meta']";
        public Info info { get; set; } = new Info();
    }
    internal class Info
    {
        public string path { get; set; } = "div[@class='video-meta-panel']/div/div[@class='column']/nav/div[@class='panel-block']";
        public string type { get; set; } = "strong";

    }
    public class Proxy
    {
        public bool enabled { get; set; } = false;
        public string? host { get; set; }
        public int port { get; set; } = 80;
        public string? userId { get; set; }
        public string? password { get; set; }
        public override string ToString()
        {
            return $"{host}:{port}&userid={userId}&password={password}";
        }
    }
}
