using System.Runtime.Serialization;

namespace PolyusTestApp.Models.FileRequests
{
    [DataContract]
    public class RenameFileRequest : FileRequestBase
    {
        [DataMember] 
        public string NewFileName { get; set; }
    }
}
