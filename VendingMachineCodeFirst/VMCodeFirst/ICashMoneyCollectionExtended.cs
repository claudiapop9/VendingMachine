using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public interface ICashMoneyCollectionExtended:ICashMoneyCollection
    {
       IList<CashMoney> GetCashMoney();
    }
}
