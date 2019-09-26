using System.Collections.Generic;

namespace VendingMachineCodeFirst.Repository
{
    public interface ICardRepositoryExtended : ICardRepository
    {
        IList<Account> GetAccounts();
    }
}
