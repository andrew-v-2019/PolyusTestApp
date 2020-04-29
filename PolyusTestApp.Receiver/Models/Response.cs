using System;
using System.Runtime.Serialization;


namespace PolyusTestApp.Receiver.Models
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string ResponseStatus { get; set; }
    }
}
