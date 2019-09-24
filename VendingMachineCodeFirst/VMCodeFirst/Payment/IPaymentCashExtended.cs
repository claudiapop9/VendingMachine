using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineCodeFirst.Payment
{
    public interface IPaymentCashExtended:IPayment
    {
        IList<CashMoney> GetMoney();
    }
}
