using System;
using System.Collections.Generic;

namespace VendingMachineCodeFirst.Service
{
    public class CashPayment : IPayment
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICashMoneyRepository cashMoneyRepository;
        private IList<CashMoney> introducedMoney = new List<CashMoney>();
        private double totalMoney = 0;

        public CashPayment(IList<CashMoney> cashMoney, double totalMoney)
        {
            this.introducedMoney = cashMoney;
            this.totalMoney = totalMoney;
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

        public bool IsValid()
        {
            return cashMoneyRepository.IsValid(this.introducedMoney);
        }
    }
}
