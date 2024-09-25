using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace JavDB.Film
{
    public class FilmInformation : IComparable<FilmInformation>
    {
        public string? Title { get; set; } // 标题
        public string? UID { get; set; } // 番号
        /// <summary>
        /// 番号系列
        /// </summary>
        public string? SeriesNumber
        {
            get
            {
                if (UID != null)
                {
                    return UID.Split("-")[0];
                }
                else
                    return null;
            }
        }
        public string? Index { get; set; } // 主页地址
        public string? Date { get; set; } // 发布日期
        public string? Backdrop { get; set; } // 海报图片地址（大）
        public string? Level { get; set; } // 评级
        public string? Poster { get; set; } // 封面图片地址（小）
        public string? Durations { get; set; } // 時長，分钟
        public string? Director { get; set; } // 導演
        public string? FilmDistributor { get; set; } // 片商
        public string? Issue { get; set; } // 發行
        public string? Series { get; set; } // 系列
        public string? Score { get; set; } // 評分
        [NonSerialized]
        public string? Magnet; // 磁力链接
        public List<string> Category { get; set; } = new List<string>(); // 類別
        public List<FilmActor> Actor { get; set; } = new List<FilmActor>(); // 演員
        public string? PreviewVideo { get; set; } // 预览视频地址
        [NonSerialized]
        public string? SRC; // 数据来源地址
        public string? GrabTime { get; set; } // 爬取时间
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, Grappler.SerializerOptions);
        }
        public static FilmInformation? Convert(string json)
        {
            return JsonSerializer.Deserialize<FilmInformation>(json, Grappler.SerializerOptions);
        }

        public int CompareTo(FilmInformation? other)
        {
            if (other == null) return -1;
            //if (this.Score == null && other.Score == null) return 0;
            //if (this.Score == null && other.Score != null) return 1;
            //if (this.Score != null && other.Score == null) return -1;
            if (double.Parse(this.Score == null ? "0" : Score) > double.Parse(other.Score == null ? "0" : other.Score))
            {
                return -1;
            }
            else if (double.Parse(this.Score == null ? "0" : Score) < double.Parse(other.Score == null ? "0" : other.Score))
            {
                return 1;
            }
            else
            {
                if (DateTime.Parse(this.Date == null ? "1971-1-1" : Date) > DateTime.Parse(other.Date == null ? "1971-1-1" : other.Date))
                {
                    return -1;
                }
                else if (DateTime.Parse(this.Date == null ? "1971-1-1" : Date) < DateTime.Parse(other.Date == null ? "1971-1-1" : other.Date))
                {
                    return 1;
                }
            }
            return 0;
        }
    }

}
