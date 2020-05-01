using PolyusTestApp.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace PolyusTestApp.Sender
{
    public class Watcher
    {
        private FileSystemWatcherRegEx FileSystemWatcher { get; }

        public delegate void FileActionHandler(FileActionData fileActionData);

        public event FileActionHandler FileActionEvent;

        private bool _enabled = true;

        public Watcher(string path, string mask)
        {
            if (!string.IsNullOrEmpty(mask))
            {
                var regEx = new Regex(mask);

                FileSystemWatcher = new FileSystemWatcherRegEx(path, regEx)
                {
                    NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                };
            }
            else
            {
                FileSystemWatcher = new FileSystemWatcherRegEx(path)
                {
                    NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                };
            }


            FileSystemWatcher.Deleted += Deleted;
            FileSystemWatcher.Created += Created;
            FileSystemWatcher.Changed += Changed;
            FileSystemWatcher.Renamed += Renamed;
        }

        public void Start()
        {
            Logger.Logger.Log("FileSystemWatcher started");
            FileSystemWatcher.EnableRaisingEvents = true;

            while (_enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            Logger.Logger.Log("FileSystemWatcher stopped");
            FileSystemWatcher.EnableRaisingEvents = false;
            _enabled = false;
            FileSystemWatcher.Dispose();
        }

        private void Renamed(object sender, RenamedEventArgs e)
        {
            var message = $"{e.OldFullPath} renamed to {e.FullPath}";
            Logger.Logger.Log(message);
            InvokeFileActionEvent(FileActionType.ChangeName, e.OldName, e.Name);
        }

        private void Changed(object sender, FileSystemEventArgs e)
        {
            var message = $"{e.FullPath} was edited";
            Logger.Logger.Log(message);
            InvokeFileActionEvent(FileActionType.Update, e.Name);
        }

        private void Created(object sender, FileSystemEventArgs e)
        {
            var message = $"{e.FullPath} was created";
            Logger.Logger.Log(message);

            InvokeFileActionEvent(FileActionType.Create, e.Name);
        }

        private void Deleted(object sender, FileSystemEventArgs e)
        {
            var message = $"{e.FullPath} was removed";
            Logger.Logger.Log(message);
            InvokeFileActionEvent(FileActionType.Delete, e.Name);
        }

        private void InvokeFileActionEvent(FileActionType type, string fullName, string newName = null)
        {
            var data = CreateFileActionData(type, fullName, newName);
            FileActionEvent?.Invoke(data);
        }

        private static FileActionData CreateFileActionData(FileActionType type, string fileName, string newName = null)
        {
            var data = new FileActionData
            {
                FileActionType = type,
                FileName = fileName,
                NewFileName = newName
            };

            return data;
        }
    }
}
