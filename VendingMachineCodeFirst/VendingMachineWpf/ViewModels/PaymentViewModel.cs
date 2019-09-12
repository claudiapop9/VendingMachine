
using System.Collections.Generic;
using Newtonsoft.Json;
using VendingMachineWpf.Models;

namespace VendingMachineWpf.ViewModels
{
    public class PaymentViewModel : ObservableObject
    {
        private List<CashMoney> introducedMoney = new List<CashMoney>();
        private double _total;
        private double _inserted;
        private double _change;


        public PaymentViewModel()
        {
            Total = 0;
            Inserted = 0;
            Change = 0;
        }

        public double Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }

        public double Inserted
        {
            get { return _inserted; }
            set
            {
                _inserted = value;
                OnPropertyChanged("Inserted");
            }
        }

        public double Change
        {
            get { return _change; }
            set
            {
                _change = value;
                OnPropertyChanged("Change");
            }
        }


        public void Insert(double value)
        {
            CashMoney cashMoney = new CashMoney(value, 1);
            introducedMoney.Add(cashMoney);
            Inserted += value;
        }

        public void SelectedPrice(double value)
        {
            Total = value;
        }

        public string GetIntroducedMoney()
        {
            return JsonConvert.SerializeObject(introducedMoney);
        }

    }
}
