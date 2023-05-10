using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursova_Ships.Classes
{
    //Клас для логування данних
    public static class Logger
    {
        private static string Source = @"D:\Cursova\Cursova_Ships\Cursova_Ships\LogFile\LogFile.log";

        public static void WritrLog(string message)
        {
            using (StreamWriter writer = new StreamWriter(Source, true))
            {
                writer.WriteLine($"{DateTime.Now} : {message}");
            }
        }
    }
}
