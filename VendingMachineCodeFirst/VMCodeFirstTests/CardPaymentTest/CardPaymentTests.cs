using Moq;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;
using VMCodeFirstTests.CardRepositoryTest;
using VendingMachineCodeFirst.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VMCodeFirstTests.CardPaymentTest
{
    [TestClass]
    public class CardPaymentTests
    {
        IList<Account> accounts;
        Mock<ICardRepositoryExtended> MockCardRepository;
        private CardPayment cardPayment;

        [TestInitialize]
        public void TestInit()
        {
            accounts = new List<Account>()
            {
                new Account {AccountId = 0, CardNO = "CardNO1", Pin="6789", Amount = 10},
                new Account {AccountId = 1, CardNO = "CardNO2", Pin="1234", Amount = 50},
                new Account {AccountId = 2, CardNO = "CardNO3", Pin="2456", Amount = 15},
            };
            MockCardRepository = new Mock<ICardRepositoryExtended>();
            cardPayment = new CardPayment(MockCardRepository.Object);
        }

        [TestMethod]
        public void Pay()
        {
            CardRepositoryMoq.UpdateAmount(MockCardRepository, accounts, "CardNO1");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            cardPayment.Pay(9.5);
            IList<Account> foundAccounts = MockCardRepository.Object.GetAccounts();

            double amountValue = foundAccounts.Where(account => account.CardNO == "CardNO1").FirstOrDefault().Amount;

            Assert.AreEqual(0.5, amountValue);
        }

        [TestMethod]
        public void IsEnough_WhenAmount_IsEnough_ReturnsTrue()
        {
            CardRepositoryMoq.IsEnough(MockCardRepository, accounts, "CardNO1");
            Assert.AreEqual(true, cardPayment.IsEnough(10));
        }

        [TestMethod]
        public void IsEnough_WhenAmount_NotEnough_ReturnsFalse()
        {
            CardRepositoryMoq.IsEnough(MockCardRepository, accounts, "CardNO1");
            Assert.AreEqual(false, cardPayment.IsEnough(15));
        }

    }

}
