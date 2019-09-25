using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public interface ICashMoneyRepositoryExtended :ICashMoneyRepository
    {
       IList<CashMoney> GetCashMoney();
    }
}
