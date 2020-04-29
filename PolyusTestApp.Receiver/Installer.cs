using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace PolyusTestApp.Receiver
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public Installer()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.NetworkService
            };

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = false;

            serviceInstaller.DisplayName = "PolyusTestApp.ReceiverWindowsService";

            serviceInstaller.ServiceName = "PolyusTestApp.ReceiverWindowsService";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

            AfterInstall += Installer_AfterInstall;
        }

        private static void Installer_AfterInstall(object sender, InstallEventArgs e)
        {
            using (var sc = new ServiceController("PolyusTestApp.ReceiverWindowsService"))
            {
                sc.Start();
            }
        }
    }
}
