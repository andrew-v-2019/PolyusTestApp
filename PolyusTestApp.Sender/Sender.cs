using System;
using System.ServiceProcess;
using System.Threading;

namespace PolyusTestApp.Sender
{
    public partial class Sender : ServiceBase
    {
        //C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe PolyusTestApp.Receiver.exe
        private Logger.Logger _logger;
        private Watcher _watcher;
        public Sender()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        private static string GetLogFileName()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var date = DateTime.Now.ToString().Replace(".", "_").Replace(":", "_");
            var res = $"{path}log-{date}.txt";
            return res;
        }

        public void Process()
        {
            var logFileName = GetLogFileName();
            EventLog.WriteEntry("logFileName = " + logFileName);
            _logger = new Logger.Logger(logFileName);
            _watcher = new Watcher("C:\\tmp", "", _logger);
            var thread = new Thread(new ThreadStart(_watcher.Start));
            thread.Start();
        }

        protected override void OnStart(string[] args)
        {
            Process();
        }

        protected override void OnStop()
        {
            _watcher.Stop();

        }
    }
}
