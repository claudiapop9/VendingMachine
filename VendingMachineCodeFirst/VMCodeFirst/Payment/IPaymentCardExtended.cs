using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public interface ICardServiceExtended : IPayment
    {
        IList<Account> GetAccounts();
        bool IsValidCard(string cardNumber, string cardPin);
    }
}
