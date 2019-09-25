using Moq;
using System.Collections.Generic;
using System.Linq;
using VendingMachineCodeFirst;

namespace VMCodeFirstTests.CashMoneyCollectionTest
{
    public static class CashMoneyRepositoryMoq
    {
        public static void UpdateMoney(Mock<ICashMoneyRepositoryExtended > MockCashMoneyCollection, IList<CashMoney> money)
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

        public static void GiveChange(Mock<ICashMoneyRepositoryExtended > MockCashMoneyCollection, IList<CashMoney> money)
        {
            MockCashMoneyCollection.Setup(mock => mock.GiveChange(It.IsAny<double>())).Callback(
                (double change) =>
                {
                    IList<CashMoney> changedMoney = CashMoneyRepository.CalculateMinimum(money, change);
                    foreach (CashMoney coinFromChange in changedMoney)
                    {
                        CashMoney cash = money.Where(x => x.MoneyValue == coinFromChange.MoneyValue).FirstOrDefault();
                        money.Remove(cash);
                        cash.Quantity -= coinFromChange.Quantity;
                        money.Add(cash);
                    }
                });
        }

        public static void GetMoney(Mock<ICashMoneyRepositoryExtended > MockCashMoneyCollection, IList<CashMoney> money)
        {
            MockCashMoneyCollection.Setup(mock => mock.GetCashMoney()).Returns(money);
        }
    }
}
