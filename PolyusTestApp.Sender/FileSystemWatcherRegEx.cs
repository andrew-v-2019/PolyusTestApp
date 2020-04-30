using System.IO;
using System.Text.RegularExpressions;


namespace PolyusTestApp.Sender
{
    public class FileSystemWatcherRegEx : FileSystemWatcher
    {
        public Regex RegexPattern { get; set; }

        public FileSystemWatcherRegEx(string path, Regex pattern) : base(path)
        {
            RegexPattern = pattern;
        }

        public FileSystemWatcherRegEx(string path, string filter) : base(path, filter) { }

        public FileSystemWatcherRegEx(string path) : base(path) { }

        public FileSystemWatcherRegEx() : base() { }

        public new event FileSystemEventHandler Changed
        {
            add
            {
                IsChanged += value;
                base.Changed += FileSystemWatcherRegEx_Changed;
            }
            remove
            {
                IsChanged -= value;
                base.Created -= FileSystemWatcherRegEx_Changed;
            }
        }


        public new event FileSystemEventHandler Created
        {
            add
            {
                IsCreated += value;
                base.Created += FileSystemWatcherRegEx_Created;
            }
            remove
            {
                IsCreated -= value;
                base.Created -= FileSystemWatcherRegEx_Created;
            }
        }

        public new event FileSystemEventHandler Deleted
        {
            add
            {
                IsDeleted += value;
                base.Deleted += FileSystemWatcherRegEx_Deleted;
            }
            remove
            {
                IsDeleted -= value;
                base.Deleted -= FileSystemWatcherRegEx_Deleted;
            }
        }


        public new event RenamedEventHandler Renamed
        {
            add
            {
                IsRenamed += value;
                base.Renamed += FileSystemWatcherRegEx_Renamed;
            }
            remove
            {
                IsRenamed -= value;
                base.Renamed -= FileSystemWatcherRegEx_Renamed;
            }
        }

        private event FileSystemEventHandler IsChanged;

        private event FileSystemEventHandler IsCreated;

        private event FileSystemEventHandler IsDeleted;

        private event RenamedEventHandler IsRenamed;

        private void FileSystemWatcherRegEx_Changed(object sender, FileSystemEventArgs e)
        {
            if (RegexPattern == null)
                IsChanged?.Invoke(sender, e);
            else if (Matches(e.Name))
                IsChanged?.Invoke(sender, e);
        }

        private void FileSystemWatcherRegEx_Created(object sender, FileSystemEventArgs e)
        {
            if (RegexPattern == null)
                IsCreated?.Invoke(sender, e);
            else if (Matches(e.Name))
                IsCreated?.Invoke(sender, e);
        }

        private void FileSystemWatcherRegEx_Deleted(object sender, FileSystemEventArgs e)
        {
            if (RegexPattern == null)
                IsDeleted?.Invoke(sender, e);
            else if (Matches(e.Name))
                IsDeleted?.Invoke(sender, e);
        }

        private void FileSystemWatcherRegEx_Renamed(object sender, RenamedEventArgs e)
        {
            if (RegexPattern == null)
                IsRenamed?.Invoke(sender, e);
            else if (Matches(e.Name))
                IsRenamed?.Invoke(sender, e);
        }

        private bool Matches(string file)
        {
            return RegexPattern.IsMatch(file);
        }
    }
}
