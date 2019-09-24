using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public interface IPaymentCardExtended : IPayment
    {
        IList<Account> GetAccounts();
        bool IsValidCard(string cardNumber, string cardPin);
    }
}
