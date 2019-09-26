using Moq;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;
using VMCodeFirstTests.CashMoneyRepositoryTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VMCodeFirstTests.CashMoneyPaymentTest
{
    [TestClass]
    public class CashMoneyServiceTests
    {
        Mock<ICashMoneyRepositoryExtended> MockCashMoneyRepository;
        private CashPayment cashPayment;

        [TestInitialize]
        public void TestInit()
        {
            IList<CashMoney> money = new List<CashMoney>()
            {
                new CashMoney() { Id = 1, MoneyValue = 10, Quantity = 10 },
                new CashMoney() { Id = 2, MoneyValue = 5, Quantity = 10 },
                new CashMoney() { Id = 3, MoneyValue = 1, Quantity = 10 },
                new CashMoney() { Id = 4, MoneyValue = 0.5, Quantity = 10 }
            };
            MockCashMoneyRepository = new Mock<ICashMoneyRepositoryExtended>();
            CashMoneyRepositoryMoq.GetMoney(MockCashMoneyRepository, money);
            CashMoneyRepositoryMoq.UpdateMoney(MockCashMoneyRepository, money);
            CashMoneyRepositoryMoq.GiveChange(MockCashMoneyRepository, money);

            IList<CashMoney> introducedMoney = new List<CashMoney>()
            {
                new CashMoney() { Id = 2, MoneyValue = 5, Quantity = 1},
                new CashMoney() { Id = 3, MoneyValue = 1, Quantity = 4},
                new CashMoney() { Id = 4, MoneyValue = 0.5, Quantity = 1 }
            };
            double total = 9.5;
            cashPayment = new CashPayment(MockCashMoneyRepository.Object, introducedMoney, total);
        }

        [TestMethod]
        public void PayWhenCostEqualsTotalAmount()
        {
            cashPayment.Pay(9.5);
            IList<CashMoney> foundMoney = MockCashMoneyRepository.Object.GetCashMoney();

            int quantityValue5 = foundMoney.Where(cash => cash.MoneyValue == 5).FirstOrDefault().Quantity;
            int quantityValue1 = foundMoney.Where(cash => cash.MoneyValue == 1).FirstOrDefault().Quantity;
            int quantityValue05 = foundMoney.Where(cash => cash.MoneyValue == 0.5).FirstOrDefault().Quantity;

            Assert.AreEqual(11, quantityValue5);
            Assert.AreEqual(14, quantityValue1);
            Assert.AreEqual(11, quantityValue05);
        }

        [TestMethod]
        public void PayWhenCostSmallerThanTotalAmount()
        {
            cashPayment.Pay(8);
            IList<CashMoney> foundMoney = MockCashMoneyRepository.Object.GetCashMoney();

            int quantityValue5 = foundMoney.Where(cash => cash.MoneyValue == 5).FirstOrDefault().Quantity;
            int quantityValue1 = foundMoney.Where(cash => cash.MoneyValue == 1).FirstOrDefault().Quantity;
            int quantityValue05 = foundMoney.Where(cash => cash.MoneyValue == 0.5).FirstOrDefault().Quantity;

            Assert.AreEqual(11, quantityValue5);
            Assert.AreEqual(13, quantityValue1);
            Assert.AreEqual(10, quantityValue05);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            MockCashMoneyRepository = null;
            cashPayment = null;
        }
    }
}
