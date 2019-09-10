using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace VendingMachineCodeFirst
{
    public class CashService : IPayment
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        List<double> acceptedDenominations = new List<double>() {10, 5, 1, 0.5};
        private List<CashMoney> introducedMoney = new List<CashMoney>();
        double totalMoney = 0;

        public CashService(List<CashMoney> introducedMoney)
        {
            this.introducedMoney = introducedMoney;
            foreach (var m in introducedMoney)
            {
                totalMoney += m.MoneyValue * m.Quantity;
            }
        }

        public void Pay(double cost)
        {
            if (cost.Equals(totalMoney))
            {
                foreach (CashMoney entry in introducedMoney)
                {
                    double value = (double) entry.MoneyValue;
                    int quantity = (Int32) entry.Quantity;
                    UpdateMoney(value, quantity);
                }
            }
            else if (cost < totalMoney)
            {
                foreach (CashMoney entry in introducedMoney)
                {
                    double value = (double) entry.MoneyValue;
                    int quantity = (Int32) entry.Quantity;
                    UpdateMoney(value, quantity);
                }

                GiveChange(totalMoney - cost);
            }
        }

        public bool IsEnough(double cost)
        {
            return cost <= totalMoney;
        }

        private void UpdateMoney(double value, int quantity)
        {
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    CashMoney cashMoney = db.Money.Where(x => x.MoneyValue == value).FirstOrDefault();
                    cashMoney.Quantity += quantity;
                    db.Entry(cashMoney).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                log.Error("Fail database connection\n");
            }
        }

        private void GiveChange(double change)
        {
            List<CashMoney> money = new List<CashMoney>();
            List<CashMoney> changedMoney = new List<CashMoney>();
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    money = db.Money.ToList<CashMoney>();
                    changedMoney = CalculateMinimum(money, change);
                    Console.WriteLine("Change: ");
                    for (int i = 0; i < changedMoney.Count; i++)
                    {
                        CashMoney coinFromChange = changedMoney[i];
                        Console.Write(changedMoney[i] + " ");
                        CashMoney cashMoney = db.Money.Where(x => x.MoneyValue == coinFromChange.MoneyValue)
                            .FirstOrDefault();
                        cashMoney.Quantity -= coinFromChange.Quantity;
                        db.Entry(cashMoney).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    log.Info("GIVE Change success");
                }
            }
            catch (Exception)
            {
                log.Error("Fail database connection-GIVE Change");
            }
        }

        private static List<CashMoney> CalculateMinimum(List<CashMoney> coins, double change)
        {
            List<CashMoney> minimalMatch = null;
            int minimalCount = -1;

            List<CashMoney> subset = coins;
            for (int i = 0; i < coins.Count; i++)
            {
                List<CashMoney> matches = Calculate(subset, change);
                if (matches != null)
                {
                    int matchCount = matches.Sum(c => (Int32) c.Quantity);
                    if (minimalMatch == null || matchCount < minimalCount)
                    {
                        minimalMatch = matches;
                        minimalCount = matchCount;
                    }
                }

                subset = subset.Skip(1).ToList();
            }

            return minimalMatch;
        }

        private static List<CashMoney> Calculate(List<CashMoney> coins, double change, int start = 0)
        {
            for (int i = start; i < coins.Count; i++)
            {
                CashMoney coin = coins[i];

                if (coin.Quantity > 0 && coin.MoneyValue <= change)
                {
                    double moneyValue = (Double) coin.MoneyValue;
                    double remainder = change % moneyValue;
                    if (remainder < change)
                    {
                        double s = (change - remainder) / moneyValue;
                        int quantity = (Int32) coin.Quantity;
                        int howMany = (Int32) Math.Min(quantity, s);

                        List<CashMoney> matches = new List<CashMoney>();
                        matches.Add(new CashMoney(moneyValue, howMany));

                        double amount = howMany * moneyValue;
                        double changeLeft = change - amount;
                        if (changeLeft == 0)
                        {
                            return matches;
                        }

                        List<CashMoney> subCalc = Calculate(coins, changeLeft, i + 1);
                        if (subCalc != null)
                        {
                            matches.AddRange(subCalc);
                            return matches;
                        }
                    }
                }
            }

            return null;
        }
    }
}