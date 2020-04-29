using System;
using System.Configuration;
using System.IO;
using PolyusTestApp.Receiver.Models;

namespace PolyusTestApp.Receiver
{
    public class Receiver : IReceiver
    {
        public Response Create(CreateFileRequest file)
        {
            var fileTransferResponse = CheckFileTransferRequest(file);
            if (fileTransferResponse.ResponseStatus != "FileIsValid")
                return fileTransferResponse;

            try
            {
                var path = ConfigurationManager.AppSettings["SavedLocation"] +
                           "\\" + file.FileName;
                SaveFileStream(path, new MemoryStream(file.Content));
                return new Response
                {
                    Date = DateTime.Now,
                    FileName = file.FileName,
                    Message = "File was transfered",
                    ResponseStatus = "Successful"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Date = DateTime.Now,
                    FileName = file.FileName,
                    Message = ex.Message,
                    ResponseStatus = "Error"
                };
            }
        }

        private static void SaveFileStream(string filePath, Stream stream)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                stream.CopyTo(fileStream);
                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Response CheckFileTransferRequest(CreateFileRequest fileToPush)
        {
            if (fileToPush == null)
                return new Response
                {
                    Date = DateTime.Now,
                    FileName = "No Name",
                    Message = " File Can't be Null",
                    ResponseStatus = "Error"
                };

            if (string.IsNullOrEmpty(fileToPush.FileName))
                return new Response
                {
                    Date = DateTime.Now,
                    FileName = "No Name",
                    Message = " File Name Can't be Null",
                    ResponseStatus = "Error"
                };

            if (fileToPush.Content != null)
            {
                return new Response
                {
                    Date = DateTime.Now,
                    FileName = fileToPush.FileName,
                    Message = string.Empty,
                    ResponseStatus = "FileIsValid"
                };
            }

            return new Response
            {
                Date = DateTime.Now,
                FileName = "No Name",
                Message = " File Content is null",
                ResponseStatus = "Error"
            };

        }
    }
}
