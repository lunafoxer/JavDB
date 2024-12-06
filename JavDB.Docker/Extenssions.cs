using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavDB.Docker
{
    public static class Extenssions
    {
        public static void Error(this Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now}][Exception]Message:{ex.Message}.StackTrace:{ex.StackTrace}");
            if (ex.InnerException != null)
                Console.WriteLine($"[InnerException]Message:{ex.InnerException.Message}.StackTrace:{ex.InnerException.StackTrace}.Source:{ex.InnerException.Source}");
            else
                Console.WriteLine("没有InnerException");
        }
    }
}
