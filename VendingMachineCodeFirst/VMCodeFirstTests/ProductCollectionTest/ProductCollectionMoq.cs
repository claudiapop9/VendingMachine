using Moq;
using System.Collections.Generic;
using System.Linq;
using VendingMachineCodeFirst;

namespace VMCodeFirstTests.ProductCollectionTest
{
    public static class ProductCollectionMoq
    {
        public static void AddProduct(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.AddProduct(It.IsAny<Product>())).Callback(
                (Product addedProduct) =>
                {
                    products.Add(addedProduct);
                });
        }

        public static void UpdateProduct(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.AddProduct(It.IsAny<Product>())).Callback(
                (Product addedProduct) =>
                {
                    Product toUpdate = products.FirstOrDefault(product => product.ProductId == addedProduct.ProductId);
                    if (toUpdate == default(Product))
                        throw new System.Exception("Product doesn't exists");
                    products.Remove(toUpdate);
                    products.Add(addedProduct);
                });
        }

        public static void RemoveProduct(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.RemoveProduct(It.IsAny<int>())).Callback(
                (int deletedProduct) =>
                {
                    Product toDelete = products.FirstOrDefault(product => product.ProductId == deletedProduct);
                    if (toDelete == default(Product))
                        throw new System.Exception();
                    products.Remove(toDelete);
                });
        }

        //public static void GetProductPriceByKey(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        //{
        //    MockProductCollection.Setup(mock => mock.GetProducts()).Returns(products);
        //}

        public static void GetProductsToRefill(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.GetProductsToRefill()).Returns(products);
        }

        public static void GetProducts(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.GetProducts()).Returns(products);
        }


    }
}
