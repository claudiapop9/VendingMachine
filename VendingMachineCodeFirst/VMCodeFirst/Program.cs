using System.Threading;

[assembly: log4net.Config.XmlConfigurator(Watch =true)]

namespace VendingMachineCodeFirst
{
    class Program
    {
        public static void CommunicateAdminThread() {
            ConnectAdmin admin = new ConnectAdmin();
            admin.Communicate();
        }

        static void Main(string[] args)
        {
            ThreadStart adminref = new ThreadStart(CommunicateAdminThread);
            Thread adminThread = new Thread(adminref);
            adminThread.Start();

            UI ui = new UI();
            ui.Run();
        }
    }
}
