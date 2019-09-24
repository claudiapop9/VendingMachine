using Moq;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VMCodeFirstTests.CashMoneyCollectionTest
{
    [TestClass]
    public class CashMoneyCollectionTests
    {
        IList<CashMoney> money;
        Mock<ICashMoneyCollectionExtended> MockCashMoneyCollection;

        [TestInitialize]
        public void TestInit()
        {
            money = new List<CashMoney>()
            {
                new CashMoney() { Id = 1, MoneyValue = 10, Quantity = 10 },
                new CashMoney() { Id = 2, MoneyValue = 5, Quantity = 10 },
                new CashMoney() { Id = 3, MoneyValue = 1, Quantity = 10 },
                new CashMoney() { Id = 4, MoneyValue = 0.5, Quantity = 10 }
            };

            MockCashMoneyCollection = new Mock<ICashMoneyCollectionExtended>();
        }

        [TestMethod]
        public void GiveChange()
        {
            CashMoneyCollectionMoq.GiveChange(MockCashMoneyCollection, money);
            CashMoneyCollectionMoq.GetMoney(MockCashMoneyCollection, money);

            MockCashMoneyCollection.Object.GiveChange(2.5);

            IList<CashMoney> foundMoney = MockCashMoneyCollection.Object.GetCashMoney();

            int quantityValue1 = foundMoney.Where(cash => cash.MoneyValue == 1).FirstOrDefault().Quantity;
            int quantityValue05 = foundMoney.Where(cash => cash.MoneyValue == 0.5).FirstOrDefault().Quantity;

            Assert.AreEqual(8, quantityValue1);
            Assert.AreEqual(9, quantityValue05);
        }

        [TestMethod]
        public void UpdateMoney()
        {
            CashMoneyCollectionMoq.UpdateMoney(MockCashMoneyCollection, money);
            CashMoneyCollectionMoq.GetMoney(MockCashMoneyCollection, money);

            MockCashMoneyCollection.Object.UpdateMoney(10, 2);

            IList<CashMoney> foundMoney = MockCashMoneyCollection.Object.GetCashMoney();

            int quantityValue = foundMoney.Where(cash => cash.MoneyValue == 10).FirstOrDefault().Quantity;

            Assert.AreEqual(12, quantityValue);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            money = null;
            MockCashMoneyCollection = null;
        }
    }
}
