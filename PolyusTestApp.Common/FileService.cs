using System.IO;

namespace PolyusTestApp.Common
{
    public class FileService
    {
        public string CalculateCheckSum(byte[] byteToCalculate)
        {
            var checksum = 0;
            foreach (var chData in byteToCalculate)
            {
                checksum += chData;
            }

            checksum &= 0xff;
            var res = checksum.ToString("X2");
            return res;
        }

        public void CreateFolder(string path)
        {
            var di = new DirectoryInfo(path);
            if (di.Exists)
                return;

            di.Create();

        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void Rename(string old, string newPath)
        {
            File.Move(old, newPath);
        }
    }
}
