using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JavDB.Extentions.Nfo
{
    public class movie
    {
        public string? id { get; set; }
        public string? plot { get; set; }
        public bool lockdata { get; set; }
        public DateTime? dateadded { get; set; }
        public string? title { get; set; }
        public string? originaltitle { get; set; }
        public string? director { get; set; }
        public List<string>? writer { get; set; }
        public List<string>? credits { get; set; }
        public List<string>? trailer { get; set; }
        public string? rating { get; set; }
        public int? year { get; set; }
        public string? mpaa { get; set; }
        public string? imdbid { get; set; }
        public string? rmdbid { get; set; }
        public DateTime? premiered { get; set; }
        public DateTime? releasedate { get; set; }
        public int? runtime { get; set; }
        public string? country { get; set; }
        public List<string>? genre { get; set; }
        public List<string>? studio { get; set; }
        public List<string>? tag { get; set; }
        public List<actor>? actor { get; set; }
        public art? art { get; set; }
        public static movie Read(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
              return Read(fs);
            }
        }
        public static movie Read(Stream st)
        {
            XmlDocument dom = new XmlDocument();
            dom.Load(st);
            if (dom.DocumentElement == null) throw new Exception("null of document");
            var movie = new movie();
            var root = dom.SelectSingleNode("/movie");
            if (root == null) throw new Exception("不是合法的数据流");
            Type t = movie.GetType();
            var pp = t.GetProperties();
            foreach (var p in pp)
            {
                Type pt = p.PropertyType;
                if (pt == typeof(List<string>))
                {
                    var nodes = root.SelectNodes($"{p.Name}");
                    if (nodes != null)
                    {
                        List<string> value = new List<string>();
                        foreach (XmlNode node in nodes)
                        {
                            value.Add(node.InnerText);
                        }
                        p.SetValue(movie, value);
                    }
                }
                else if (pt == typeof(List<actor>))
                {
                    var nodes = root.SelectNodes($"{p.Name}");
                    if (nodes != null)
                    {
                        List<actor> value = new List<actor>();
                        foreach (XmlNode node in nodes)
                        {
                            actor at = new actor();
                            Type aType = at.GetType();
                            var ap = aType.GetProperties();
                            foreach (var i in ap)
                            {
                                var it = node.SelectSingleNode($"{i.Name}");
                                if (it != null)
                                {
                                    SetValue(at, i, it.InnerText);
                                }

                            }
                            value.Add(at);
                        }
                        p.SetValue(movie, value);
                    }
                }
                else
                {
                    var node = root.SelectSingleNode($"{p.Name}");
                    if (node != null)
                    {
                        object? value;
                        if (pt == typeof(art))
                        {
                            art at = new art();
                            Type aType = at.GetType();
                            var ap = aType.GetProperties();
                            foreach (var i in ap)
                            {
                                SetValue(at, i, node.SelectSingleNode($"{i.Name}")?.InnerText);
                            }
                            p.SetValue(movie, at);
                        }
                        else
                        {
                            SetValue(movie, p, node.InnerText);
                        }
                    }
                }
            }
            return movie;
        }
        public static void Write(string filename, movie mv)
        {
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Write(fs, mv);
            }
        }
        public static void Write(Stream st, movie mv)
        {
            using BinaryWriter fs = new BinaryWriter(st);
            byte[] buffer = Encoding.UTF8.GetBytes(mv.ToString());
            fs.BaseStream.SetLength(0);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
        }
        private static void SetValue(object obj, PropertyInfo p, object value)
        {
            if (obj == null) return;
            if (value == null) return;
            Type pt = p.PropertyType;
            object? v;
            if (pt == typeof(DateTime?) || pt == typeof(DateTime))
            {
                v = DateTime.Parse(value.ToString());
            }
            else if (pt == typeof(bool))
            {
                v = bool.Parse(value.ToString());
            }
            else if (pt == typeof(int?) || pt == typeof(int))
            {
                v = int.Parse(value.ToString());
            }
            else
            {
                v = value.ToString();
            }
            p.SetValue(obj, v);
        }
        public override string ToString()
        {
            Type t = this.GetType();
            var pp = t.GetProperties();
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xml.CreateXmlDeclaration("1.0", "utf-8", "yes");
            xml.AppendChild(xmldecl);
            XmlElement xmlelem = xml.CreateElement("", "movie", "");
            foreach (var p in pp)
            {
                var value = p.GetValue(this);
                if (value != null)
                {
                    var flag = xml.CreateDocumentFragment();
                    if (value.GetType() == typeof(List<actor>))
                    {
                        foreach (var item in (List<actor>)value)
                        {
                            if (item != null)
                            {
                                flag.InnerXml = $"<actor>{item.ToString()}</actor>";
                                xmlelem.AppendChild(flag);
                            }
                        }
                    }
                    else if (value.GetType() == typeof(List<string>))
                    {
                        foreach (var item in (List<string>)value)
                        {
                            if (item != null)
                            {
                                flag.InnerXml = $"<{p.Name}>{item.ToString()}</{p.Name}>";
                                xmlelem.AppendChild(flag);
                            }
                        }
                    }
                    else if (value.GetType() == typeof(DateTime) && p.Name != "dateadded")
                    {
                        flag.InnerXml = $"<{p.Name}>{((DateTime)value).ToString("yyyy-MM-dd")}</{p.Name}>";
                        xmlelem.AppendChild(flag);
                    }
                    else
                    {
                        flag.InnerXml = $"<{p.Name}>{value.ToString()}</{p.Name}>";
                        xmlelem.AppendChild(flag);
                    }

                }
            }
            xml.AppendChild(xmlelem);
            return xml.InnerXml;
        }
    }
}
