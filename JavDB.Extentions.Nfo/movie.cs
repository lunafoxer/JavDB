using System.Diagnostics;
using System.Xml;

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
        public ushort? year { get; set; }
        public string? mpaa { get; set; }
        public string? imdbid { get; set; }
        public string? rmdbid { get; set; }
        public DateTime? premiered { get; set; }
        public DateTime? releasedate { get; set; }
        public ushort? runtime { get; set; }
        public string? country { get; set; }
        public List<string>? genre { get; set; }
        public List<string>? studio { get; set; }
        public List<string>? tag { get; set; }
        public List<actor>? actors { get; set; }
        public art? art { get; set; }
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
                    else if(value.GetType()==typeof(DateTime) && p.Name!= "dateadded")
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
