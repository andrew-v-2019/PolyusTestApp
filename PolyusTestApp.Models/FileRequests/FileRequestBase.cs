using System.Runtime.Serialization;

namespace PolyusTestApp.Models.FileRequests
{
    [DataContract]
    public class FileRequestBase
    {
        [DataMember]
        public string FileName { get; set; }
    }
}
