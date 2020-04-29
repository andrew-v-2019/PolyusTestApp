using System.ServiceModel;
using PolyusTestApp.Receiver.Models;


namespace PolyusTestApp.Receiver
{
    [ServiceContract]
    public interface IReceiver
    {
        [OperationContract]
        Response Create(CreateFileRequest file);
    }
}
