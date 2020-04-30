using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PolyusTestApp.InstallClient
{
    class Program
    {
        private const string ServiceName = "PolyusTestApp.SenderWindowsService";

        static readonly string execFileName = "PolyusTestApp.Sender.exe";

        static void Main(string[] args)
        {
            const string startCommand = "echo \"install receiver\"";

            var stopCommand = $"net stop {ServiceName}";

            var delCommand = $"sc delete {ServiceName}";

            var execFile = GetServiceExecFilePath();

            var instalUtilPath = GetInstalUtilPath();
            var installCommand = $"{instalUtilPath} {execFile}";
            var commands = new[] {startCommand, stopCommand, delCommand, installCommand};
            RunProcess(commands);
        }

        private static void RunProcess(string[] commands)
        {
            var cmd = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {string.Join("& ", commands)}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            cmd.Start();
            var output = cmd.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            Console.ReadLine();
        }

        private static string GetServiceExecFilePath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var allFoundFiles = Directory.GetFiles(path, $"{execFileName}", SearchOption.AllDirectories);

            if (allFoundFiles == null || !allFoundFiles.Any())
            {
                throw new Exception($"{execFileName}.exe not found");
            }

            return allFoundFiles.First();
        }

        private static string GetInstalUtilPath()
        {
            var is64BitSystem = Environment.Is64BitOperatingSystem;
            var winDir = Environment.GetEnvironmentVariable("windir");

            var utilPath = is64BitSystem
                ? $"{winDir}\\Microsoft.NET\\Framework64\\v4.0.30319\\InstallUtil.exe"
                : $"{winDir}\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe";

            if (!File.Exists(utilPath) || string.IsNullOrEmpty(winDir))
            {
                throw new Exception("InstallUtil.exe not found");
            }

            return utilPath;
        }
    }
}
