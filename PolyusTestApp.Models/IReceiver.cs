using System.ServiceModel;
using PolyusTestApp.Models.FileRequests;

namespace PolyusTestApp.Models
{
    [ServiceContract]
    public interface IReceiver
    {
        [OperationContract]
        Response Create(CreateFileRequest file);

        [OperationContract]
        Response Delete(FileRequestBase file);

        [OperationContract]
        Response RenameFile(RenameFileRequest request);

        [OperationContract]
        Response Update(CreateFileRequest request);
    }
}
