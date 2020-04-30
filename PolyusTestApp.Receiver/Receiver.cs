using System;
using System.Configuration;
using System.IO;
using PolyusTestApp.Common;
using PolyusTestApp.Models;
using PolyusTestApp.Models.FileRequests;


namespace PolyusTestApp.Receiver
{
    public class Receiver : IReceiver
    {
        private readonly FileService _fileService;
        public Receiver()
        {
            _fileService = new FileService();
        }

        private void CreateSyncFolder()
        {
            var folder = ConfigurationManager.AppSettings["SyncFolder"];
            _fileService.CreateFolder(folder);
        }

        public Response Create(CreateFileRequest request)
        {
            var fileTransferResponse = CheckFileRequest(request);
            if (fileTransferResponse.ResponseStatus != true)
                return fileTransferResponse;
            CreateSyncFolder();
            var path = GetFullFilePath(request.FileName);

            try
            {
                Logger.Logger.Log($"About to create {path}");
                var stream = new MemoryStream(request.Content);
                SaveFileStream(path, stream);
                return CreateResponse(request.FileName, "File was created", true);
            }
            catch (Exception ex)
            {
                Logger.Logger.Log(
                    $"Exception while creation {path}, exception is {ex.Message}, inner exception is {ex.InnerException}");
                return CreateResponse(request.FileName, ex.Message, false);
            }
        }

        public Response Update(CreateFileRequest request)
        {
            var fileTransferResponse = CheckFileRequest(request);
            if (fileTransferResponse.ResponseStatus != true)
                return fileTransferResponse;
            CreateSyncFolder();
            var path = GetFullFilePath(request.FileName);

            try
            {
                Logger.Logger.Log($"About to update {path}");
                var stream = new MemoryStream(request.Content);
                SaveFileStream(path, stream);
                return CreateResponse(request.FileName, "File was updated", true);
            }
            catch (Exception ex)
            {
                Logger.Logger.Log(
                    $"Exception while updating {path}, exception is {ex.Message}, inner exception is {ex.InnerException}");
                return CreateResponse(request.FileName, ex.Message, false);
            }
        }

        public Response Delete(FileRequestBase request)
        {
            var fileTransferResponse = CheckFileRequest(request);
            if (fileTransferResponse.ResponseStatus != true)
                return fileTransferResponse;

            var path = GetFullFilePath(request.FileName);

            try
            {
                Logger.Logger.Log($"About to delete {path}");
                _fileService.DeleteFile(path);
                return CreateResponse(request.FileName, "File was deleted", true);
            }
            catch (Exception ex)
            {
                Logger.Logger.Log(
                    $"Exception while removing {path}, exception is {ex.Message}, inner exception is {ex.InnerException}");
                return CreateResponse(request.FileName, ex.Message, false);
            }
        }

        public Response RenameFile(RenameFileRequest request)
        {
            var fileTransferResponse = CheckFileRequest(request);
            if (fileTransferResponse.ResponseStatus != true)
                return fileTransferResponse;

            var path = GetFullFilePath(request.FileName);
            var newPath = GetFullFilePath(request.NewFileName);

            try
            {
                Logger.Logger.Log($"About to rename {path} to {newPath}");
                _fileService.Rename(path, newPath);
                return CreateResponse(request.FileName, $"File {path} was renamed to {newPath}", true);
            }
            catch (Exception ex)
            {
                Logger.Logger.Log(
                    $"Exception while removing {path}, exception is {ex.Message}, inner exception is {ex.InnerException}");
                return CreateResponse(request.FileName, ex.Message, false);
            }
        }


        private void SaveFileStream(string filePath, Stream stream)
        {
            _fileService.DeleteFile(filePath);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
        }

        private static Response CheckFileRequest(FileRequestBase file)
        {
            if (file == null)
                return CreateResponse(string.Empty, "File can not be Null", false);

            return string.IsNullOrEmpty(file.FileName)
                ? CreateResponse(file.FileName, "File Name can not be Null", false)
                : CreateResponse(file.FileName, "File Is Valid", true);
        }

        private Response CheckFileRequest(CreateFileRequest file)
        {
            var baseStatus = CheckFileRequest((FileRequestBase)file);
            if (baseStatus.ResponseStatus == false)
            {
                return baseStatus;
            }

            var localCheckSum = _fileService.CalculateCheckSum(file.Content);

            if (localCheckSum != file.CheckSum)
            {
                return CreateResponse(file.FileName, "CheckSum was failed", false);
            }

            return file.Content != null
                ? CreateResponse(file.FileName, "File Is Valid", true)
                : CreateResponse(file.FileName, "File Content is null", false);
        }

        private static Response CheckFileRequest(RenameFileRequest file)
        {
            var baseStatus = CheckFileRequest((FileRequestBase)file);
            if (baseStatus.ResponseStatus == false)
            {
                return baseStatus;
            }

            return !string.IsNullOrWhiteSpace(file.NewFileName)
                ? CreateResponse(file.FileName, "File Is Valid", true)
                : CreateResponse(file.FileName, "New file name can not be null", false);
        }

        private static Response CreateResponse(string fileName, string message, bool status)
        {
            return new Response(fileName, message, status);
        }

        private static string GetFullFilePath(string name)
        {
            var path = ConfigurationManager.AppSettings["SyncFolder"] +
                       "\\" + name;

            return path;
        }
    }
}
