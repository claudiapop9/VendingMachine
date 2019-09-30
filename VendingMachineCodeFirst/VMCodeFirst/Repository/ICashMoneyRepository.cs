using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public interface ICashMoneyRepository
    {
        void UpdateMoney(double value, int quantity);
        void GiveChange(double change);
        bool IsValid(IList<CashMoney> money);
    }
}
