using System;
using System.Linq;
using VendingMachineCommon;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;
using VMCodeFirst.Controller;

namespace VendingMachineCodeFirst
{
    class UI
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IPayment payment;
        ClientService service;
        ClientController clientCtrl;
        CashController cashCtrl;

        public void Run()
        {
            while (true)
            {
                clientCtrl = new ClientController(new ClientService());
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
            IList<Product> products = clientCtrl.GetProducts();

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
                    ShowProductList();
                    string id = ChooseProduct();
                    cashCtrl = new CashController();
                    Tuple<IList<CashMoney>, double> cashDetails = AskCashMoney(id);
                    payment = new CashPayment(cashDetails.Item1, cashDetails.Item2);
                    ShopMenu(id);
                    break;
                case "2":
                    log.Info("Card Payment");
                    ShowProductList();
                    Tuple<string, string> cardDetails = AskCardDetails();
                    payment = new CardPayment(cardDetails.Item1, cardDetails.Item2);
                    ShopMenu(ChooseProduct());
                    break;
                default:
                    Console.WriteLine("Invalid command, try again");
                    WayOfPayment();
                    break;
            }
        }

        private Tuple<string, string> AskCardDetails()
        {
            Console.WriteLine("CardNo:");
            string cardNo = Console.ReadLine();
            Console.WriteLine("PIN:");
            string pin = Console.ReadLine();
            return new Tuple<string, string>(cardNo, pin);
        }

        private Tuple<IList<CashMoney>, double> AskCashMoney(string id)
        {
            try
            {
                double cost = clientCtrl.GetProducts().Where(product => product.ProductId.ToString() == id).FirstOrDefault().Price;
                Console.WriteLine("Introduce money:");
                double money = Double.Parse(Console.ReadLine());
                cashCtrl.AddMoney(money);
                while (cashCtrl.TotalMoney < cost)
                {
                    Console.WriteLine($"Not enough.Introduce more: {cost - cashCtrl.TotalMoney}:");
                    money = Double.Parse(Console.ReadLine());
                    cashCtrl.AddMoney(money);
                };
                return new Tuple<IList<CashMoney>, double>(cashCtrl.IntroducedMoney, cashCtrl.TotalMoney);
            }
            catch (Exception e)
            {
                log.Error("Something is wrong at cash payment.");
                Console.WriteLine(e);
                return new Tuple<IList<CashMoney>, double>(new List<CashMoney>(), 0);
            }
        }

        private void ShopMenu(string id)
        {
            try
            {
                service = new ClientService(payment);
                clientCtrl = new ClientController(service);
                if (clientCtrl.BuyProduct(id))
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
        private string ChooseProduct()
        {
            Console.WriteLine("Product id:");
            string id = Console.ReadLine();
            return id;
        }

    }
}