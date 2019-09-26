using Moq;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;
using VendingMachineCodeFirst.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VMCodeFirstTests.CardRepositoryTest
{
    [TestClass]
    public class CardRepositoryTests
    {
        IList<Account> accounts;
        Mock<ICardRepositoryExtended> MockCardRepository;

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
        }

        [TestMethod]
        public void UpdateAmount()
        {
            CardRepositoryMoq.UpdateAmount(MockCardRepository, accounts, "CardNO1");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            MockCardRepository.Object.UpdateAmount(10);
            IList<Account> foundedAccounts = MockCardRepository.Object.GetAccounts();

            double amount = foundedAccounts.FirstOrDefault(account => account.CardNO == "CardNO1").Amount;
            Assert.AreEqual(0, amount);
        }

        [TestMethod]
        public void isEnoughWhenCostLowerThanAmmount()
        {
            CardRepositoryMoq.IsEnough(MockCardRepository, accounts, "CardNO1");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            bool isEnough = MockCardRepository.Object.IsEnough(5);

            Assert.AreEqual(true, isEnough);
        }

        [TestMethod]
        public void isEnoughWhenCosEqualWithAmmount()
        {
            CardRepositoryMoq.IsEnough(MockCardRepository, accounts, "CardNO2");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            bool isEnough = MockCardRepository.Object.IsEnough(50);

            Assert.AreEqual(true, isEnough);
        }

        [TestMethod]
        public void isEnoughWhenCostBiggerThanAmmount()
        {
            CardRepositoryMoq.IsEnough(MockCardRepository, accounts, "CardNO3");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            bool isEnough = MockCardRepository.Object.IsEnough(16);

            Assert.AreEqual(false, isEnough);
        }

        [TestMethod]
        public void isValidCardWhenValidCard()
        {
            CardRepositoryMoq.IsValidCard(MockCardRepository, accounts, "CardNO1", "6789");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            bool isValid = MockCardRepository.Object.IsValidCard();
            Assert.AreEqual(true, isValid);
        }

        [TestMethod]
        public void isValidCardWhen_ValidCardNO_InvalidPin()
        {
            CardRepositoryMoq.IsValidCard(MockCardRepository, accounts, "CardNO1", "6666");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            bool isValid = MockCardRepository.Object.IsValidCard();
            Assert.AreEqual(false, isValid);
        }

        [TestMethod]
        public void isValidCardWhen_InvalidCard()
        {
            CardRepositoryMoq.IsValidCard(MockCardRepository, accounts, "InvalidCard", "6666");
            CardRepositoryMoq.GetAccounts(MockCardRepository, accounts);

            bool isValid = MockCardRepository.Object.IsValidCard();
            Assert.AreEqual(false, isValid);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            accounts = null;
            MockCardRepository = null;
        }
    }
}
