using JavDB.Film;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JavDB.Polling
{
    internal class Config
    {
        public string CachePath { get; set; } = Path.Combine(Application.StartupPath, "cache");
        public int Delay { get; set; } = 1000;
        public byte Mode { get; set; } = 0;
        public string[] AutoComplete { get; set; } = new string[2] { "/video_codes/", "/actors/" };
        public double Score { get; set; } = 8.0;
        public string Player { get; set; } = "http://m.karevin.cn:5100/res/player.htm";
        private ushort mExpirationTime = 720;
        public ushort ExpirationTime
        {
            get => mExpirationTime;
            set
            {
                mExpirationTime = value;
                ExpirationSeconds = (long)value * 3600;
            }
        }
        public long ExpirationSeconds = 2592000;
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, Grappler.SerializerOptions);
        }
        public void Save(string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(Encoding.UTF8.GetBytes(this.ToString()));
                }
            }
        }
    }
}
