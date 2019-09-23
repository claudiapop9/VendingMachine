﻿using Moq;
using System;
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
            MockProductCollection.Setup(mock => mock.UpdateProduct(It.IsAny<Product>())).Callback(
               (Product addedProduct) =>
               {
                   Product toUpdate = products.FirstOrDefault(product => product.ProductId == addedProduct.ProductId);
                   if (toUpdate == default(Product))
                       throw new Exception("Product doesn't exists");
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
                        throw new Exception();
                    products.Remove(toDelete);
                });
        }

        public static void DecreaseProductQuantity(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.DecreaseProductQuantity(It.IsAny<int>())).Callback(
               (int decreasedQuantityProduct) =>
               {
                   Product toDecrease = products.FirstOrDefault(product => product.ProductId == decreasedQuantityProduct);
                   if (toDecrease == default(Product))
                       throw new Exception();

                   Product updatedProduct = toDecrease;
                   updatedProduct.Quantity -= 1;

                   products.Remove(toDecrease);
                   products.Add(updatedProduct);
               });
        }

        public static void GetProductPriceByKey(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.GetProductPriceByKey(It.IsAny<int>())).Returns(
                (int id) =>
                {
                    Product product = products.SingleOrDefault(p => p.ProductId == id);
                    if (product == default(Product))
                        return -1;
                    return product.Price;
                });
        }

        public static void GetProductsToRefill(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.GetProductsToRefill()).Returns(products.Where(product => product.Quantity < 10).ToList());
        }

        public static void GetProducts(Mock<IProductCollection> MockProductCollection, IList<Product> products)
        {
            MockProductCollection.Setup(mock => mock.GetProducts()).Returns(products);
        }
    }
}
