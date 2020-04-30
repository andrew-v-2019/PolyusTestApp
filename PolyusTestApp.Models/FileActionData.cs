
namespace PolyusTestApp.Models
{
    public class FileActionData
    {
        public FileActionType FileActionType { get; set; }

        public string FileName { get; set; }

        public string NewFileName { get; set; }


        public override string ToString()
        {
            return
                $"FileActionType = {FileActionType.ToString()}, FileFullName={FileName}, NewFileName = {NewFileName}";
        }
    }
}
