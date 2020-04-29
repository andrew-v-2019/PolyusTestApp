using System;
using System.IO;

namespace PolyusTestApp.Logger
{
    public class Logger
    {
        private readonly string _logFileFullName;
        private readonly object _lck = new object();

        public Logger(string logFileFullName)
        {
            _logFileFullName = logFileFullName;
        }

        public void Log(string message)
        {
            lock (_lck)
            {

                using (var writer = new StreamWriter(_logFileFullName, true))
                {
                    var date = DateTime.Now;
                    writer.WriteLine($"{date}: {message}");
                    writer.Flush();
                }
            }
        }
    }
}
