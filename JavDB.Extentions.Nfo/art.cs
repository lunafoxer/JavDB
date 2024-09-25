using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavDB.Extentions.Nfo
{
    public class art
    {
        public string? poster{ get; set; }
        public string? fanart { get; set; }
        public override string ToString()
        {
            Type t = this.GetType();
            var pp = t.GetProperties();
            string result = "";
            foreach (var p in pp)
            {
                var value = p.GetValue(this);
                if (value != null)
                {
                    result += $"<{p.Name}>{value.ToString()}</{p.Name}>";
                }
            }
            return result;
        }
    }
}
