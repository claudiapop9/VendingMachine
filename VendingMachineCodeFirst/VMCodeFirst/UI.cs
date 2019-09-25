using System;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;

namespace VendingMachineCodeFirst
{
    class UI
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPayment payment;
        ClientService service;
        Controller ctrl;
        Validator validator = new Validator();


        public void Run()
        {
            while (true)
            {
                ctrl = new Controller(new ClientService());
                MainMenu();
            }
        }

        private void MainMenu()
        {
            string str = "------------Menu----------------\n\n";
            str += "1.List of products\n";
            str += "2.Buy product\n";

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
                default:
                    Console.WriteLine("Invalid option :( Try again");
                    MainMenu();
                    break;
            }
        }

        private void ShowProductList()
        {
            IList<Product> products = ctrl.GetProducts();

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine(products[i]);
            }

            Console.ReadKey();
        }

        private void WayOfPayment()
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
                    AskCardDetails();
                    ShopMenu();
                    break;
                default:
                    Console.WriteLine("Invalid command, try again");
                    WayOfPayment();
                    break;
            }
        }
        private void AskCardDetails()
        {
            Console.WriteLine("CardNo:");
            string cardNo = Console.ReadLine();
            Console.WriteLine("PIN:");
            string pin = Console.ReadLine();
            payment = new CardPayment(cardNo, pin);
        }

        private void ShopMenu()
        {
            ShowProductList();
            try
            {
                Console.WriteLine("Product id:");
                string id = Console.ReadLine();
                service = new ClientService(payment);
                ctrl = new Controller(service);
                if (ctrl.BuyProduct(id))
                {
                    Console.WriteLine("Product bought successfully :D");
                    ShowProductList();
                    log.Info("Product bought successfully");
                }
                Console.WriteLine("The product wasn't bought :( \n");
                Console.ReadKey();
                log.Info("The Product wasn't bought :( \n");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}