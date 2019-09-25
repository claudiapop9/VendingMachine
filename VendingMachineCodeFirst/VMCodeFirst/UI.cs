using System;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;

namespace VendingMachineCodeFirst
{
    class UI
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Controller ctrl;
        Validator validator = new Validator();
        IPayment payment;

        public void Run()
        {
            while (true)
            {
                ctrl = new Controller();
                MainMenu();
            }
        }

        public void MainMenu()
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

        public void ShowProductList()
        {
            IList<Product> products = ctrl.GetProducts();

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
                    payment = new CashService();
                    ShopMenu();
                    break;
                case "2":
                    log.Info("Card Payment");
                    payment = new CardService();
                    ShopMenu();
                    break;
                default:
                    Console.WriteLine("Invalid command, try again");
                    WayOfPayment();
                    break;
            }
        }

        public void ShopMenu()
        {
            ShowProductList();
            try
            {
                Console.WriteLine("Product id:");
                string id = Console.ReadLine();
                ctrl = new Controller(payment);
                if (ctrl.BuyProduct(id))
                {
                    Console.WriteLine("Product bought successfully :D");
                    ShowProductList();
                    log.Info("Product bought successfully");
                    return;
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