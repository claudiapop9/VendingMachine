using Moq;
using System;
using System.Linq;
using VendingMachineCodeFirst;
using System.Collections.Generic;
using VendingMachineCodeFirst.Repository;

namespace VMCodeFirstTests.CardRepositoryTest
{
    public static class CardRepositoryMoq
    {
        public static void UpdateAmount(Mock<ICardRepositoryExtended> MockCardRepository, IList<Account> accounts, string cardNo)
        {
            MockCardRepository.Setup(mock => mock.UpdateAmount(It.IsAny<double>())).Callback(
               (double cost) =>
               {
                   Account toUpdate = accounts.FirstOrDefault(account => account.CardNO == cardNo);
                   if (toUpdate == default(Account))
                       throw new Exception("Account doesn't exists");

                   accounts.Remove(toUpdate);
                   toUpdate.Amount -= cost;
                   accounts.Add(toUpdate);
               });
        }

        public static void IsEnough(Mock<ICardRepositoryExtended> MockCardRepository, IList<Account> accounts, string cardNo)
        {
            MockCardRepository.Setup(mock => mock.IsEnough(It.IsAny<double>())).Returns(
                (double cost) =>
                {
                    Account account = accounts.FirstOrDefault(acc => acc.CardNO == cardNo);
                    if (account.Amount >= cost)
                    {
                        return true;
                    }
                    return false;
                });
        }

        public static void IsValidCard(Mock<ICardRepositoryExtended> MockCardRepository, IList<Account> accounts, string cardNo, string pin)
        {
            MockCardRepository.Setup(mock => mock.IsValidCard()).Returns(
                () =>
                {
                    Account account = accounts.FirstOrDefault(acc => acc.CardNO == cardNo && acc.Pin == pin);
                    if (account == default(Account))
                    {
                        return false;
                    }
                    return true;
                });
        }

        public static void GetAccounts(Mock<ICardRepositoryExtended> MockCardRepository, IList<Account> accounts)
        {
            MockCardRepository.Setup(mock => mock.GetAccounts()).Returns(accounts);
        }
    }
}
