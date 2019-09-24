using Moq;
using System;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;

namespace VMCodeFirstTests.CashMoneyCollectionTest
{
    public static class CashMoneyCollectionMoq
    {
        public static void UpdateMoney(Mock<ICashMoneyCollectionExtended> MockCashMoneyCollection, IList<CashMoney> money)
        {
            MockCashMoneyCollection.Setup(mock => mock.UpdateMoney(It.IsAny<double>(), It.IsAny<int>())).Callback(
               (double value, int quantity) =>
               {
                   CashMoney cashMoney = money.Where(x => x.MoneyValue == value).FirstOrDefault();
                   money.Remove(cashMoney);
                   cashMoney.Quantity += quantity;
                   money.Add(cashMoney);
               });
        }

        public static void GiveChange(Mock<ICashMoneyCollectionExtended> MockCashMoneyCollection, IList<CashMoney> money)
        {
            MockCashMoneyCollection.Setup(mock => mock.GiveChange(It.IsAny<double>())).Callback(
                (double change) =>
                {
                    IList<CashMoney> changedMoney = CalculateMinimum(money, change);
                    foreach (CashMoney coinFromChange in changedMoney)
                    {
                        CashMoney cash = money.Where(x => x.MoneyValue == coinFromChange.MoneyValue).FirstOrDefault();
                        money.Remove(cash);
                        cash.Quantity -= coinFromChange.Quantity;
                        money.Add(cash);
                    }
                });
        }

        private static IList<CashMoney> CalculateMinimum(IList<CashMoney> coins, double change)
        {
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

        public static void GetMoney(Mock<ICashMoneyCollectionExtended> MockCashMoneyCollection, IList<CashMoney> money)
        {
            MockCashMoneyCollection.Setup(mock => mock.GetCashMoney()).Returns(money);
        }
    }
}
