using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace PolyusTestApp.Sender
{
    [RunInstaller(true)]
    public partial class SenderServiceInstaller : Installer
    {
        private const string ServiceName = "PolyusTestApp.SenderWindowsService";

        public SenderServiceInstaller()
        {
            InitializeComponent();
            var serviceInstaller = new ServiceInstaller();
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.NetworkService
            };

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = false;

            serviceInstaller.DisplayName = ServiceName;

            serviceInstaller.ServiceName = ServiceName;
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

            InitializeComponent();

            AfterInstall += SenderServiceInstaller_AfterInstall;
        }

        private static void SenderServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (var sc = new ServiceController(ServiceName))
            {
                sc.Start();
            }
        }
    }
}
