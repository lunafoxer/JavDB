﻿using JavDB.Film;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JavDB.Client
{
    internal class Config
    {
        public string CachePath { get; set; } = Path.Combine(Application.StartupPath, "cache");
        public string PlayerURL = "file:///" + Path.Combine(Application.StartupPath, "player.htm");
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
