using System.ServiceModel;
using System.ServiceProcess;


namespace PolyusTestApp.Receiver
{
    public class ReceiverWindowsService: ServiceBase
    {
        public ServiceHost ServiceHost = null;
        public ReceiverWindowsService()
        {
            ServiceName = "PolyusTestApp.ReceiverWindowsService";
        }

        public static void Main(string[] args)
        {
            ServiceBase.Run(new ReceiverWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            ServiceHost?.Close();

            ServiceHost = new ServiceHost(typeof(Receiver));
            ServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (ServiceHost == null) return;
            ServiceHost.Close();
            ServiceHost = null;
        }

        private void InitializeComponent()
        {
            // 
            // ReceiverWindowsService
            // 
            this.ServiceName = "PolyusTestApp.ReceiverWindowsService";

        }
    }
}
