using System.Runtime.Serialization;

namespace PolyusTestApp.Models.FileRequests
{
    [DataContract]
    public class CreateFileRequest : FileRequestBase
    {
        [DataMember] 
        public byte[] Content { get; set; }

        [DataMember] 
        public string CheckSum { get; set; }
    }
}
