using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineWpf.Models
{
    class CashMoney
    {
        public double MoneyValue { get; set; }
        public int Quantity { get; set; }

        public CashMoney(double moneyValue, int quantity)
        {
            MoneyValue = moneyValue;
            Quantity = quantity;
        }
    }
}
