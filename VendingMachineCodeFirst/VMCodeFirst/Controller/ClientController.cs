using System;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;
using VendingMachineCommon;

namespace VendingMachineCodeFirst
{
    public class ClientController
    {
        private ClientService service;
        private Validator validator = new Validator();
        private IList<double> acceptedDenominations = new List<double>() { 10, 5, 1, 0.5 };
        public IList<CashMoney> IntroducedMoney { get; set; } = new List<CashMoney>() { };
        public double TotalMoney { get; set; } = 0;

        public ClientController(ClientService service)
        {
            this.service = service;
        }

        public bool BuyProduct(string id)
        {
            if (validator.isIdValid(id))
            {
                int productId = Int32.Parse(id);
                return service.BuyProduct(productId);
            }
            return false;
        }
        public void AddMoney(double money)
        {
            if (acceptedDenominations.Contains(money))
            {
                CashMoney cashMoney = new CashMoney(money, 1);
                IntroducedMoney.Add(cashMoney);
                TotalMoney += money;
            }
            else
            {
                throw new Exception("Money not accepted");
            }
        }

        public IList<Product> GetProducts()
        {
            return service.GetProducts();
        }
    }
}