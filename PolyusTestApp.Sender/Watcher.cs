using System;
using System.IO;
using System.Threading;

namespace PolyusTestApp.Sender
{
    public class Watcher
    {
        private readonly Logger.Logger _logger;
        private FileSystemWatcher FileSystemWatcher { get; }

        bool _enabled = true;

        public Watcher(string path, string mask, Logger.Logger logger)
        {
            _logger = logger;
            FileSystemWatcher = new FileSystemWatcher(path);

            FileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                             | NotifyFilters.LastWrite
                                             | NotifyFilters.FileName;


            FileSystemWatcher.Deleted += Deleted;
            FileSystemWatcher.Created += Created;
            FileSystemWatcher.Changed += Changed;
            FileSystemWatcher.Renamed += Renamed;
        }

        public void Start()
        {
            _logger.Log("FileSystemWatcher started");
            FileSystemWatcher.EnableRaisingEvents = true;

            while(_enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            _logger.Log("FileSystemWatcher stopped");
            FileSystemWatcher.EnableRaisingEvents = false;
            _enabled = false;
            FileSystemWatcher.Dispose();
        }


        private void Renamed(object sender, RenamedEventArgs e)
        {
            var message = $"{e.OldFullPath} renamed to {e.FullPath}";
            _logger.Log(message);
        }

        private void Changed(object sender, FileSystemEventArgs e)
        {
            var message = $"{e.FullPath} was edited";
            _logger.Log(message);
        }

        private void Created(object sender, FileSystemEventArgs e)
        {
            var message = $"{e.FullPath} was created";
            _logger.Log(message);
        }

        private void Deleted(object sender, FileSystemEventArgs e)
        {
            var message = $"{e.FullPath} was removed";
            _logger.Log(message);
        }
    }
}
