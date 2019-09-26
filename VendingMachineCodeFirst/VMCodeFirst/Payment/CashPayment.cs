using System;
using System.Collections.Generic;

namespace VendingMachineCodeFirst.Service
{
    public class CashPayment : IPayment
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICashMoneyRepository cashMoneyRepository;
        private IList<CashMoney> introducedMoney = new List<CashMoney>();
        private IList<double> acceptedDenominations = new List<double>() { 10, 5, 1, 0.5 };
        private double totalMoney = 0;

        public CashPayment()
        {
            this.cashMoneyRepository = new CashMoneyRepository();
        }
        public CashPayment(ICashMoneyRepository moneyCollection)
        {
            this.cashMoneyRepository = moneyCollection;
        }
        public CashPayment(ICashMoneyRepository moneyCollection, IList<CashMoney> introducedMoney, double totalMoney)
        {
            this.cashMoneyRepository = moneyCollection;
            this.introducedMoney = introducedMoney;
            this.totalMoney = totalMoney;
        }
        public void Pay(double cost)
        {
            if (cost.Equals(totalMoney))
            {
                foreach (CashMoney entry in introducedMoney)
                {
                    double value = (double)entry.MoneyValue;
                    int quantity = (Int32)entry.Quantity;
                    cashMoneyRepository.UpdateMoney(value, quantity);
                }
            }
            else if (cost < totalMoney)
            {
                foreach (CashMoney entry in introducedMoney)
                {
                    double value = (double)entry.MoneyValue;
                    int quantity = (Int32)entry.Quantity;
                    cashMoneyRepository.UpdateMoney(value, quantity);
                }
                cashMoneyRepository.GiveChange(totalMoney - cost);
            }
        }

        public bool IsEnough(double cost)
        {
            return cost <= totalMoney;
        }

        public void AskForMoney(double cost)
        {
            try
            {
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
                log.Error("Wrong introduced money.");
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

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
