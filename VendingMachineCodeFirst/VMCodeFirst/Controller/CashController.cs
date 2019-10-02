using System;
using VendingMachineCodeFirst;
using System.Collections.Generic;

namespace VMCodeFirst.Controller
{
    public class CashController
    {
        private IList<double> acceptedDenominations = new List<double>() { 10, 5, 1, 0.5 };
        public IList<CashMoney> IntroducedMoney { get; set; } = new List<CashMoney>() { };
        public double TotalMoney { get; set; } = 0;

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

    }
}
