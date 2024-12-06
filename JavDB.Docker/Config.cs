using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavDB.Docker
{
    internal class Config
    {
        public List<string> ListenPath { get; set; }
        public int Delay { get; set; } = 1000;
        public List<string> FileExtensions { get; set; }

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
        public Config()
        {
            string listenFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "listen.txt");
            ListenPath = new List<string>();
            if (File.Exists(listenFile))
            {
                using FileStream file = new FileStream(listenFile, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        string? line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                            ListenPath.Add(line);
                    }
                }
            }
            string extensionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "fileEx.txt");
            FileExtensions = new List<string>();
            if (File.Exists(extensionFile))
            {
                using FileStream file = new FileStream(extensionFile, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        string? line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                            FileExtensions.Add(line);
                    }
                }
            }
        }
    }
}
