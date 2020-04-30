using System;
using System.Runtime.Serialization;

namespace PolyusTestApp.Models
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
        public bool ResponseStatus { get; set; }



        public Response(string fileName, string message, bool status)
        {
            Date = DateTime.Now;
            FileName = fileName;
            Message = message;
            ResponseStatus = status;
        }

        public override string ToString()
        {
            return
                $"Date = {Date}, FileName={FileName}, Message = {Message}, ResponseStatus = {ResponseStatus}";
        }
    }
}
