using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BusinessLogic;
using Newtonsoft.Json;

namespace VendingMachineWpf.ViewModels
{
    public class MachineViewModel: ObservableObject
    {
        public string CardCredentials = null;
        public PaymentViewModel Bank { get; private set; }
        public ObservableCollection<ProductViewModel> Items { get; private set; }
        public Controller ctrl = new Controller();

        public MachineViewModel()
        {
            Bank = new PaymentViewModel();
            Items = GetObservableCollection();
        }

        private ObservableCollection<ProductViewModel> GetObservableCollection()
        {
            ObservableCollection<ProductViewModel> items = new ObservableCollection<ProductViewModel>();
            try
            {
                List<ProductViewModel> productsModels = JsonConvert.DeserializeObject<List<ProductViewModel>>(ctrl.GetProducts());
                foreach (var product in productsModels)
                {
                    items.Add(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return items;
        }

        public void PurchaseCash(object item)
        {
            var requestedItem = item as ProductViewModel;
            string insertedMoney = Bank.GetIntroducedMoney();
            ctrl.BuyProduct(requestedItem.Id, insertedMoney);
        }

        public void PurchaseCard(object item)
        {
            var requestedItem = item as ProductViewModel;
            ctrl.BuyProduct(requestedItem.Id, CardCredentials);
        }

        public void InsertChange(double value)
        {
            Bank.Insert(value);
        }

        public void InsertCredentials(string cardNo, string pin)
        {
            var cardCredentials = new
            {
                CardNo = cardNo,
                Pin = pin,
            };
            this.CardCredentials = JsonConvert.SerializeObject(cardCredentials);
        }

    }
}
