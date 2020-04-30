using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace PolyusTestApp.Sender
{
    [RunInstaller(true)]
    public partial class SenderServiceInstaller : Installer
    {
        private const string ServiceName = "PolyusTestApp.SenderWindowsService";
        readonly ServiceInstaller _serviceInstaller;
        public SenderServiceInstaller()
        {
            InitializeComponent();
            _serviceInstaller = new ServiceInstaller();
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.NetworkService
            };

            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceInstaller.DelayedAutoStart = false;

            _serviceInstaller.DisplayName = ServiceName;

            _serviceInstaller.ServiceName = ServiceName;
            Installers.Add(processInstaller);
            Installers.Add(_serviceInstaller);

            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            using (var serviceController = new ServiceController(_serviceInstaller.ServiceName))
            {
                serviceController.Start();
            }
        }
    }
}
