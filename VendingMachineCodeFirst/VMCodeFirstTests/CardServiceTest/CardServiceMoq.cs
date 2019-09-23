using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineCodeFirst;

namespace VMCodeFirstTests.CardServiceTest
{
    public static class CardServiceMoq
    {
        public static void Pay(Mock<IPaymentCardExtended> MockCardPayment, IList<Account> accounts, string cardNo)
        {
            MockCardPayment.Setup(mock => mock.Pay(It.IsAny<double>())).Callback(
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

        public static void IsEnough(Mock<IPaymentCardExtended> MockCardPayment, IList<Account> accounts, string cardNo)
        {
            MockCardPayment.Setup(mock => mock.IsEnough(It.IsAny<double>())).Returns(
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

        public static void IsValidCard(Mock<IPaymentCardExtended> MockCardPayment, IList<Account> accounts)
        {
            MockCardPayment.Setup(mock => mock.IsValidCard(It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string cardNumber, string pin) =>
                {
                    Account account = accounts.FirstOrDefault(acc => acc.CardNO == cardNumber && acc.Pin == pin);
                    if (account == default(Account))
                    {
                        return false;
                    }
                    return true;
                });
        }

        public static void GetAccounts(Mock<IPaymentCardExtended> MockCardPayment, IList<Account> accounts)
        {
            MockCardPayment.Setup(mock => mock.GetAccounts()).Returns(accounts);
        }
    }
}
