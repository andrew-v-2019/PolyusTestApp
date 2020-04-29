using System.Runtime.Serialization;

namespace PolyusTestApp.Receiver.Models
{
    [DataContract]
    public  class CreateFileRequest
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] Content { get; set; }
    }
}
