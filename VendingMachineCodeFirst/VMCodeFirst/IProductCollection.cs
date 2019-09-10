using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public interface IProductCollection
    {
        void AddProduct(Product p);

        void UpdateProduct(Product p);

        void DecreaseProductQuantity(int productId);

        void RemoveProduct(int productId);

        double GetProductPriceByKey(int id);

        bool Refill();

        IList<Product> GetProductsToRefill();

        IList<Product> GetProducts();
    }
}
