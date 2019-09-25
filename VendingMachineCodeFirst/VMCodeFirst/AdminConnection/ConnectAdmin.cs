namespace VendingMachineCodeFirst
{
    class ConnectAdmin
    {
        public void Communicate()
        {
            SocketCommunication socketCommunication = new SocketCommunication();
            if (socketCommunication.IsConnected())
            {
                System.Console.WriteLine("Socket communication established");
                AdminController ctrl = new AdminController(socketCommunication);
                ctrl.HandleCommands();
            }
            socketCommunication.ReleaseSocket();
        }
    }
}
