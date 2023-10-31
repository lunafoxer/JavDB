using System;

namespace JavDB.Film.Common
{
    public static class Extensions
    {
        public static string? Middle(this string str, string? left = null, string? right = null, bool caps = true)
        {
            if (string.IsNullOrEmpty(str)) throw new NullReferenceException("str");
            if (string.IsNullOrEmpty(left))
            {
                if (string.IsNullOrEmpty(right))
                    return str;
                else
                {
                    int index = str.IndexOf(right, caps ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                    if (index >= 0)
                    {
                        return str.Substring(0, index);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(right))
                {
                    int index = str.IndexOf(left, caps ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                    if (index >= 0)
                    {
                        int len = index + left.Length;
                        return str.Substring(len, str.Length - index);
                    }
                }
                else
                {
                    int index = str.IndexOf(left, caps ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                    if (index >= 0)
                    {
                        int len = index + left.Length;
                        int rindex = str.IndexOf(right, len, caps ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
                        if (rindex > len)
                        {
                            return str.Substring(len, rindex - len);
                        }
                    }
                }
            }
            return null;
        }
        public static string[] GetActorNames(this List<FilmActor> actors, Gender filterSex = Gender.NONE)
        {
            List<string> names = new List<string>();
            foreach (FilmActor actor in actors)
            {
                if (actor.Gender == filterSex) names.Add(actor.ToString());
            }
            return names.ToArray();
        }
        public static string? GetString(this List<string> strings, char splitChar)
        {
            return GetString(strings.ToArray(), splitChar);
        }
        public static string? GetString(this List<string> strings)
        {
            return GetString(strings.ToArray());
        }
        public static string? GetString(this string[] strings, char splitChar)
        {
            if (strings == null) return null;
            string? str = "";
            foreach (string s in strings)
            {
                if (s == null) continue;
                str += s + splitChar;
            }
            if (str.Length == 0) return str;
            return str.Substring(0, str.Length - 1);
        }
        public static string? GetString(this string[] strings)
        {
            if (strings == null) return null;
            string? str = "";
            foreach (string s in strings)
            {
                if (s == null) continue;
                str += s;
            }
            return str;
        }
        public static bool IsNullOrEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static long GetUnixTimestamp(this DateTime dt)
        {
            DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime timeUTC = DateTime.SpecifyKind(dt, DateTimeKind.Utc);//本地时间转成UTC时间
            TimeSpan ts = timeUTC - dd;
            return (long)ts.TotalSeconds;
        }
        /// <summary>
        /// 时间戳转本时区日期时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static bool TryParseUnixTimeStamp(this DateTime dt, long timeStamp, out DateTime dateTime)
        {
            try
            {
                DateTime dd = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Local);
                TimeSpan ts = new TimeSpan(timeStamp);
                dateTime = dd.Add(ts);
                return true;
            }
            catch
            {
                dateTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Local);
                return false;
            }
        }
    }
}
