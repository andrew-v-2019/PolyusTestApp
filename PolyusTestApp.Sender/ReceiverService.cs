
using System;
using System.Configuration;
using System.ServiceModel;
using PolyusTestApp.Models;
using PolyusTestApp.Models.FileRequests;

namespace PolyusTestApp.Sender
{
    public class ReceiverService
    {

        private static IReceiver CreateChanel()
        {
            var serverUrl = ConfigurationManager.AppSettings["RemoteServer"] + "/SyncFolder";
            Logger.Logger.Log($"About to create connection with {serverUrl}");
            var uri = new Uri(serverUrl);
            var address = new EndpointAddress(uri);
            var binding = new NetTcpBinding("NewBinding0");
            var factory = new ChannelFactory<IReceiver>(binding, address);
            var service = factory.CreateChannel();
            Logger.Logger.Log($"Connection with {serverUrl} has been created");
            return service;
        }

        public Response CreateFile(CreateFileRequest createRequest)
        {
            var service = CreateChanel();
            var response = service.Create(createRequest);
            return response;
        }

        public Response UpdateFile(CreateFileRequest updateRequest)
        {
            var service = CreateChanel();
            var response = service.Update(updateRequest);
            return response;
        }

        public Response DeleteFile(FileRequestBase deleteRequest)
        {
            var service = CreateChanel();
            var response = service.Delete(deleteRequest);
            return response;
        }

        public Response RenameFile(RenameFileRequest renameFileRequest)
        {
            var service = CreateChanel();
            var response = service.RenameFile(renameFileRequest);
            return response;
        }
    }
}
