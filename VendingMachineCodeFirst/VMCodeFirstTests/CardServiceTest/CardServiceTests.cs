using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using VendingMachineCodeFirst;

namespace VMCodeFirstTests.CardServiceTest
{
    [TestClass]
    public class CardServiceTests
    {
        IList<Account> accounts;
        Mock<IPaymentCardExtended> MockCardPayment;

        [TestInitialize]
        public void TestInit()
        {
            accounts = new List<Account>()
            {
                new Account {AccountId = 0, CardNO = "CardNO1", Pin="6789", Amount = 10},
                new Account {AccountId = 1, CardNO = "CardNO2", Pin="1234", Amount = 50},
                new Account {AccountId = 2, CardNO = "CardNO3", Pin="2456", Amount = 15},
            };

            MockCardPayment = new Mock<IPaymentCardExtended>();
        }

        [TestMethod]
        public void Pay()
        {
            CardServiceMoq.Pay(MockCardPayment, accounts, "CardNO1");
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            MockCardPayment.Object.Pay(10);
            IList<Account> foundedAccounts = MockCardPayment.Object.GetAccounts();

            double amount = foundedAccounts.FirstOrDefault(account => account.CardNO == "CardNO1").Amount;
            Assert.AreEqual(0, amount);
        }

        [TestMethod]
        public void isEnoughWhenCostLowerThanAmmount()
        {
            CardServiceMoq.IsEnough(MockCardPayment, accounts, "CardNO1");
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            bool isEnough = MockCardPayment.Object.IsEnough(5);

            Assert.AreEqual(true, isEnough);
        }

        [TestMethod]
        public void isEnoughWhenCosEqualWithAmmount()
        {
            CardServiceMoq.IsEnough(MockCardPayment, accounts, "CardNO2");
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            bool isEnough = MockCardPayment.Object.IsEnough(50);

            Assert.AreEqual(true, isEnough);
        }

        [TestMethod]
        public void isEnoughWhenCostBiggerThanAmmount()
        {
            CardServiceMoq.IsEnough(MockCardPayment, accounts, "CardNO3");
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            bool isEnough = MockCardPayment.Object.IsEnough(16);

            Assert.AreEqual(false, isEnough);
        }

        [TestMethod]
        public void isValidCardWhenValidCard()
        {
            CardServiceMoq.IsValidCard(MockCardPayment, accounts);
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            bool isValid = MockCardPayment.Object.IsValidCard("CardNO1", "6789");
            Assert.AreEqual(true, isValid);
        }

        [TestMethod]
        public void isValidCardWhen_ValidCardNO_InvalidPin()
        {
            CardServiceMoq.IsValidCard(MockCardPayment, accounts);
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            bool isValid = MockCardPayment.Object.IsValidCard("CardNO1", "6666");
            Assert.AreEqual(false, isValid);
        }

        [TestMethod]
        public void isValidCardWhen_InvalidCard()
        {
            CardServiceMoq.IsValidCard(MockCardPayment, accounts);
            CardServiceMoq.GetAccounts(MockCardPayment, accounts);

            bool isValid = MockCardPayment.Object.IsValidCard("InvalidCard", "6666");
            Assert.AreEqual(false, isValid);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            accounts = null;
            MockCardPayment = null;
        }
    }
}
