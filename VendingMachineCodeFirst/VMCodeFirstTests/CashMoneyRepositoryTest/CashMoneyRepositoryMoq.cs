using Moq;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;

namespace VMCodeFirstTests.CashMoneyRepositoryTest
{
    public static class CashMoneyRepositoryMoq
    {
        public static void UpdateMoney(Mock<ICashMoneyRepositoryExtended> MockCashMoneyRepository, IList<CashMoney> money)
        {
            MockCashMoneyRepository.Setup(mock => mock.UpdateMoney(It.IsAny<double>(), It.IsAny<int>())).Callback(
               (double value, int quantity) =>
               {
                   CashMoney cashMoney = money.Where(x => x.MoneyValue == value).FirstOrDefault();
                   money.Remove(cashMoney);
                   cashMoney.Quantity += quantity;
                   money.Add(cashMoney);
               });
        }

        public static void GiveChange(Mock<ICashMoneyRepositoryExtended> MockCashMoneyRepository, IList<CashMoney> money)
        {
            MockCashMoneyRepository.Setup(mock => mock.GiveChange(It.IsAny<double>())).Callback(
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

        public static void GetMoney(Mock<ICashMoneyRepositoryExtended> MockCashMoneyRepository, IList<CashMoney> money)
        {
            MockCashMoneyRepository.Setup(mock => mock.GetCashMoney()).Returns(money);
        }
    }
}
