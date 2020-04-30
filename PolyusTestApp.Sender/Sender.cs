using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using PolyusTestApp.Common;
using PolyusTestApp.Models;
using PolyusTestApp.Models.FileRequests;

namespace PolyusTestApp.Sender
{
    public partial class Sender : ServiceBase
    {
        private Watcher _watcher;
        private string _syncFolder;
        private readonly ReceiverService _receiverService;
        private readonly FileService _fileService;

        public Sender()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
            _receiverService = new ReceiverService();
            _fileService = new FileService();
        }

        public void Process()
        {
            EventLog.WriteEntry("Process method");
            EventLog.WriteEntry("logFileName = " + Logger.Logger.LogFileFullName);
            _syncFolder = ConfigurationManager.AppSettings["SyncFolder"];
            var mask = ConfigurationManager.AppSettings["Mask"];
            _watcher = new Watcher(_syncFolder, mask);
            var thread = new Thread(_watcher.Start);
            thread.Start();
            _watcher.FileActionEvent += SendFileData;
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("OnStart method");
            Logger.Logger.Log("OnStart method");
            Process();
        }

        protected override void OnStop()
        {
            _watcher.Stop();
        }

        private void SendFileData(FileActionData data)
        {
            Logger.Logger.Log($"About to send FileData = {data}");
            try
            {
                Response response;
                switch (data.FileActionType)
                {
                    case FileActionType.Create:
                    {
                        var createRequest = GetCreateFileRequest(data);
                        response = _receiverService.CreateFile(createRequest);
                        break;
                    }
                    case FileActionType.Update:
                    {
                        var updateRequest = GetCreateFileRequest(data);
                        response = _receiverService.UpdateFile(updateRequest);
                        break;
                    }
                    case FileActionType.Delete:
                    {
                        var deleteRequest = new FileRequestBase
                        {
                            FileName = data.FileName,
                        };
                        response = _receiverService.DeleteFile(deleteRequest);
                        break;
                    }
                    case FileActionType.ChangeName:
                    {
                        var renameRequest = new RenameFileRequest
                        {
                            FileName = data.FileName,
                            NewFileName = data.NewFileName
                        };
                        response = _receiverService.RenameFile(renameRequest);
                        break;
                    }
                    default:
                        return;
                }

                Logger.Logger.Log($"FileData = {data} has been sent with response = {response}");
            }
            catch (Exception ex)
            {
                Logger.Logger.Log(
                    $"Exception with FileData = {data}, exception is {ex.Message}, inner exception is {ex.InnerException}");
            }
        }

        private CreateFileRequest GetCreateFileRequest(FileActionData data)
        {
            var fileFullName = $"{_syncFolder}\\{data.FileName}";
            var bytes = File.ReadAllBytes(fileFullName);
            var checkSum = _fileService.CalculateCheckSum(bytes);
            var createRequest = new CreateFileRequest
            {
                FileName = data.FileName,
                Content = bytes,
                CheckSum = checkSum
            };

            return createRequest;
        }

    }
}
