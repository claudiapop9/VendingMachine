using System;
using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    class UI
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Controller ctrl = new Controller();
        IPayment payment;

        public void Run()
        {
            while (true)
            {
                MainMenu();
            }
        }

        public void MainMenu()
        {
            string str = "------------Menu----------------\n\n";
            str += "1.List of products\n";
            str += "2.Buy product\n";
            str += "3.Communicate with admin\n";

            Console.WriteLine(str);
            string c = Console.ReadLine();
            switch (c)
            {
                case "1":
                    ShowProductList();
                    break;
                case "2":
                    WayOfPayment();
                    break;
                case "3":
                    Communicate();
                    break;
            }
        }


        public void ShowProductList()
        {
            IList<Product> products = ctrl.GetProductsList();

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine(products[i]);
            }

            Console.ReadKey();
        }

        public void WayOfPayment()
        {
            string str = "-----------Payment----------------\n\n";
            str += "1.Cash\n";
            str += "2.By Card\n";
            Console.WriteLine(str);
            string c = Console.ReadLine();
            switch (c)
            {
                case "1":
                    log.Info("Cash Payment");
                    payment = new CashPayment();
                    ShopMenu();
                    break;
                case "2":
                    log.Info("Card Payment");
                    payment = new CardPayment();
                    ShopMenu();
                    break;
            }
        }

        public void Communicate()
        {
            SocketCommunication socketCommunication = new SocketCommunication();
            if (socketCommunication.IsConnected())
            {
                ProxyServer proxy = new ProxyServer(socketCommunication);
                proxy.HandleCommands();
            }
        }

        public void ShopMenu()
        {
            ShowProductList();
            try
            {
                Console.WriteLine("Product id:");
                int id = Int32.Parse(Console.ReadLine());
                ctrl = new Controller(payment);
                if (ctrl.BuyProduct(id))
                {
                    Console.WriteLine("Product bought successfully :D");
                    ShowProductList();
                    log.Info("Product bought successfully");
                }
                else
                {
                    Console.WriteLine("The Product wasn't bought :( \n");
                    Console.ReadKey();
                    log.Info("The Product wasn't bought :( \n");
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}