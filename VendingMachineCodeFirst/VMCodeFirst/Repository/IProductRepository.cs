using System.Collections.Generic;
using VendingMachineCommon;

namespace VendingMachineCodeFirst
{
    public interface IProductRepository
    {
        void AddProduct(Product p);

        void UpdateProduct(Product p);

        void RemoveProduct(int productId);

        void DecreaseProductQuantity(int productId);

        double GetProductPriceByKey(int id);

        bool Refill();

        IList<Product> GetProductsToRefill();

        IList<Product> GetProducts();
    }
}
