using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavDB.Film
{
    public class FilmActor
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        public Gender Gender { get; set; } = Gender.NONE;
        public override string ToString()
        {
            return $"[{Name}{(Gender == Gender.WOMAN ? "" : Gender == Gender.MALE ? "(男)" : "未知")}]";
        }
    }
    public enum Gender
    {
        NONE = 0,
        MALE = 1,
        WOMAN = 2
    }
}
