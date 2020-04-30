using System.Collections;
using System.Collections.Generic;
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

        private const string ServiceName = "PolyusTestApp.ReceiverWindowsService";

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

            serviceInstaller.DisplayName = ServiceName;

            serviceInstaller.ServiceName = ServiceName;
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

                // AfterInstall += Installer_AfterInstall;
        }

        //private static void Installer_AfterInstall(object sender, InstallEventArgs e)
        //{
        //    var serviceInstaller = (ServiceInstaller)sender;
        //    using (var sc = new ServiceController(serviceInstaller.ServiceName))
        //    {
        //        sc.Start();
        //    }
        //}

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            using (var serviceController = new ServiceController(serviceInstaller.ServiceName))
            {
                serviceController.Start();
            }
        }
    }
}
