using JavDB.Film;
using System.Collections.Generic;
using System;

namespace JavDB.Services
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Grappler grap = new Grappler(File.OpenText("JavDB.Film.json").ReadToEnd());
            //do
            //{
            //    Console.Write("请输入要抓取的UID：");
            //    string? uid = Console.ReadLine();
            //    if (!string.IsNullOrEmpty(uid))
            //    {
            //        var md = grap.Grab(uid);
            //          Console.WriteLine(md);
            //        //output_vsmeta(md);
            //    }
            //    Console.Write("\n");
            //} while (true);
            HTTPServices services = new HTTPServices("d:\\cache", "");
            // services.Satrt("http://+:8212/");
            services.Satrt("http://+:8212/");
            do
            {
                Thread.Sleep(10000);
            } while (true);
        }
        public static void Debug(string message)
        {
            Console.WriteLine($"[{DateTime.Now}][INFO]{message}");
        }
    }
}