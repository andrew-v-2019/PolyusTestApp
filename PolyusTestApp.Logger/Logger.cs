using System;
using System.IO;
using PolyusTestApp.Common;

namespace PolyusTestApp.Logger
{
    public static class Logger
    {
        public static readonly string LogFileFullName;
        private static readonly object Lck = new object();


        static Logger()
        {
            LogFileFullName = GetLogFileName();
        }

        private static void CreateLogFolder(string folder)
        {
            var di = new DirectoryInfo(folder);
            if (di.Exists)
                return;

            di.Create();
        }

        public static void Log(string message)
        {
            lock (Lck)
            {
                using (var writer = new StreamWriter(LogFileFullName, true))
                {
                    var date = DateTime.Now;
                    writer.WriteLine($"{date}: {message}");
                    writer.Flush();
                }
            }
        }

        private static string GetLogFileName()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory+"log/";
            CreateLogFolder(path);
            var date = DateTime.Now.FormatDateForFileName();
            var res = $"{path}log-{date}.txt";
            return res;
        }
    }
}
