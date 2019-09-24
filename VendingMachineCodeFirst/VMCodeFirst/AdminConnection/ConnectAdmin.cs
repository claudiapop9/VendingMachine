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
                ProxyServer proxy = new ProxyServer(socketCommunication);
                proxy.HandleCommands();
            }
            socketCommunication.ReleaseSocket();
        }
    }
}
