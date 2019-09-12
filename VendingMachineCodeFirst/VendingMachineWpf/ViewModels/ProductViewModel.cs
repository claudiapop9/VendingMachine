using System.Windows;
using VendingMachineWpf.Models;

namespace VendingMachineWpf.ViewModels
{
    public class ProductViewModel:ObservableObject
    {
        private VendingItem model;

        public int Id
        {
            get
            {
                return model.Id;
            }
        }
        public int Quantity
        {
            get { return model.Quantity; }
        }

        public VendingItem Information
        {
            get
            {
                return model;
            }
        }

        public Visibility OutOfStockMessage
        {
            get
            {
                if (Quantity > 0)
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        public ProductViewModel(int id, string name, int quantity, double price)
        {
            model = new VendingItem(id, name, quantity, price);
        }
    }
}
