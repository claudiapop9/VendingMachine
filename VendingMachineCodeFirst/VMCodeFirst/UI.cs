using System;
using System.Linq;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;

namespace VendingMachineCodeFirst
{
    class UI
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IList<double> acceptedDenominations = new List<double>() { 10, 5, 1, 0.5 };
        private IList<CashMoney> introducedMoney = new List<CashMoney>();
        double totalMoney = 0;
        IPayment payment;
        ClientService service;
        ClientController ctrl;

        public void Run()
        {
            while (true)
            {
                ctrl = new ClientController(new ClientService());
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
                    ShowProductList();
                    string id = ChooseProduct();
                    AskCashMoney(id);
                    payment = new CashPayment(introducedMoney,totalMoney);
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

        private void AskCashMoney(string id)
        {
            try
            {
                double cost = ctrl.GetProducts().Where(product => product.ProductId.ToString() == id).FirstOrDefault().Price;
                Console.WriteLine("Introduce money:");
                double money = Double.Parse(Console.ReadLine());
                AddMoney(money);
                while (totalMoney < cost)
                {
                    Console.WriteLine($"Not enough.Introduce more: {cost - totalMoney}:");
                    money = Double.Parse(Console.ReadLine());
                    AddMoney(money);
                };
            }
            catch (Exception e)
            {
                log.Error("Something is wrong at cash payment.");
                Console.WriteLine(e);
            }
        }

        public void AddMoney(double money)
        {
            if (acceptedDenominations.Contains(money))
            {
                CashMoney cashMoney = new CashMoney(money, 1);
                introducedMoney.Add(cashMoney);
                totalMoney += money;
            }
            else
            {
                throw new Exception("Money not accepted");
            }
        }

        private void ShopMenu(string id)
        {
            try
            {
                service = new ClientService(payment);
                ctrl = new ClientController(service);
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
        private string ChooseProduct()
        {
            Console.WriteLine("Product id:");
            string id = Console.ReadLine();
            return id;
        }

    }
}