using System.ServiceProcess;

namespace PolyusTestApp.Sender
{
    static class Program
    {
        
        static void Main()
        {
            //#if(!DEBUG)
            //            ServiceBase[] ServicesToRun;
            //            ServicesToRun = new ServiceBase[]
            //            {
            //                new Sender()
            //            };
            //            ServiceBase.Run(ServicesToRun);
            //#else
            //            var debugService = new Sender();
            //            debugService.Process();
            //#endif


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Sender()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
