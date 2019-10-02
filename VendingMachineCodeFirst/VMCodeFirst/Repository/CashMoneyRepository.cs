using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public class CashMoneyRepository : ICashMoneyRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void UpdateMoney(double value, int quantity)
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

        public bool IsValid(IList<CashMoney> money)
        {
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    IList<CashMoney> cashMoney = db.Money.ToList<CashMoney>();
                    foreach (CashMoney m in money) {
                        if (cashMoney.Where(mon=> mon.MoneyValue == m.MoneyValue).FirstOrDefault() == null)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                log.Error("Fail database connection-IsValid");
                return false;
            }
        }

        public void GiveChange(double change)
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
                        CashMoney cashMoney = db.Money.Where(x => x.MoneyValue == coinFromChange.MoneyValue).FirstOrDefault();
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

        public static List<CashMoney> CalculateMinimum(IList<CashMoney> coins, double change)
        {
            // used to store the minimum matches
            List<CashMoney> minimalMatch = null;
            int minimalCount = -1;

            List<CashMoney> subset = coins.ToList();
            for (int i = 0; i < coins.Count; i++)
            {
                List<CashMoney> matches = Calculate(subset, change);
                if (matches != null)
                {
                    int matchCount = matches.Sum(c => (Int32)c.Quantity);
                    if (minimalMatch == null || matchCount < minimalCount)
                    {
                        minimalMatch = matches;
                        minimalCount = matchCount;
                    }
                }
                // reduce the list of possible coins
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
                    double moneyValue = (Double)coin.MoneyValue;
                    double remainder = change % moneyValue;
                    if (remainder < change)
                    {
                        double s = (change - remainder) / moneyValue;
                        int quantity = (Int32)coin.Quantity;
                        int howMany = (Int32)Math.Min(quantity, s);

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

