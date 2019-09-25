using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using VendingMachineCodeFirst;
using VMCodeFirstTests.ProductCollectionTest;

namespace VMCodeFirstTests
{
    [TestClass]
    public class ProductionCollectionTests
    {
        IList<Product> products;
        Mock<IProductRepository> MockProductRepository;

        [TestInitialize]
        public void TestInit()
        {
            products = new List<Product>()
            {
                new Product {ProductId=0, Name = "ProdTest1", Quantity=6, Price=10},
                new Product {ProductId=1, Name = "ProdTest2", Quantity=8, Price=5 },
                new Product {ProductId=2, Name = "ProdTest3", Quantity=10, Price=15 },
            };

            MockProductRepository = new Mock<IProductRepository>();
        }

        [TestMethod]
        public void AddProduct()
        {
            ProductCollectionMoq.AddProduct(MockProductRepository, products);
            ProductCollectionMoq.GetProducts(MockProductRepository, products);

            Product p = new Product { ProductId = 0, Name = "ProdTest1a", Quantity = 4, Price = 10 };
            MockProductRepository.Object.AddProduct(p);
            IList<Product> foundProducts = MockProductRepository.Object.GetProducts();

            Assert.AreEqual(foundProducts.Count, products.Count);
        }

        [TestMethod]
        public void UpdateProduct()
        {
            ProductCollectionMoq.UpdateProduct(MockProductRepository, products);
            ProductCollectionMoq.GetProducts(MockProductRepository, products);

            Product p = new Product { ProductId = 0, Name = "ProdTest1a", Quantity = 4, Price = 10 };
            MockProductRepository.Object.UpdateProduct(p);
            IList<Product> foundProducts = MockProductRepository.Object.GetProducts();

            string updatedName = foundProducts.FirstOrDefault(product => product.ProductId == p.ProductId).Name;

            Assert.AreEqual("ProdTest1a", updatedName);
        }

        [TestMethod]
        public void RemoveProductWhenIdExists()
        {
            ProductCollectionMoq.RemoveProduct(MockProductRepository, products);
            ProductCollectionMoq.GetProducts(MockProductRepository, products);

            MockProductRepository.Object.RemoveProduct(0);

            IList<Product> foundProducts = MockProductRepository.Object.GetProducts();

            Assert.AreEqual(2, foundProducts.Count);
        }

        [TestMethod]
        public void DecreaseProductQuantityWhenIdExists()
        {
            ProductCollectionMoq.DecreaseProductQuantity(MockProductRepository, products);
            ProductCollectionMoq.GetProducts(MockProductRepository, products);

            MockProductRepository.Object.DecreaseProductQuantity(0);

            IList<Product> foundProducts = MockProductRepository.Object.GetProducts();
            int updatedQuantity = foundProducts.FirstOrDefault(product => product.ProductId == 0).Quantity;

            Assert.AreEqual(5, updatedQuantity);
        }

        [TestMethod]
        public void GetProductPriceByKeyWhenProductExists()
        {
            ProductCollectionMoq.GetProductPriceByKey(MockProductRepository, products);
            ProductCollectionMoq.GetProducts(MockProductRepository, products);

            double price = MockProductRepository.Object.GetProductPriceByKey(0);

            Assert.AreEqual(10, price);
        }

        [TestMethod]
        public void GetProductPriceByKeyWhenNoProduct()
        {
            products = new List<Product>();

            ProductCollectionMoq.GetProductPriceByKey(MockProductRepository, products);
            ProductCollectionMoq.GetProducts(MockProductRepository, products);

            double price = MockProductRepository.Object.GetProductPriceByKey(0);

            Assert.AreEqual(-1, price);
        }

        [TestMethod]
        public void RefillWhenQuantity10ReturnTrue()
        {
            ProductCollectionMoq.GetProductsToRefill(MockProductRepository, products);
            IList<Product> foundProducts = MockProductRepository.Object.GetProductsToRefill();

            Assert.AreEqual(2, foundProducts.Count);
        }

        [TestMethod]
        public void GetProductsToRefillWhenProductsWithQuantityLessThan10ReturnsProductsList()
        {
            ProductCollectionMoq.GetProductsToRefill(MockProductRepository, products);
            IList<Product> foundProducts = MockProductRepository.Object.GetProductsToRefill();

            Assert.AreEqual(2, foundProducts.Count);
        }

        [TestMethod]
        public void GetProductsToRefillWhenAllProductsQuantityAre10ReturnsEmptyList()
        {
            products = new List<Product>
            {
                new Product { Name = "ProdTest1", Quantity=10, Price=10},
                new Product { Name = "ProdTest2", Quantity=10, Price=10 },
                new Product { Name = "ProdTest3", Quantity=10, Price=10 },
            };

            ProductCollectionMoq.GetProductsToRefill(MockProductRepository, products);
            IList<Product> foundProducts = MockProductRepository.Object.GetProductsToRefill();

            Assert.AreEqual(0, foundProducts.Count);
        }

        [TestMethod]
        public void GetAllProducts()
        {
            ProductCollectionMoq.GetProducts(MockProductRepository, products);
            IList<Product> foundProducts = MockProductRepository.Object.GetProducts();

            Assert.AreEqual(3, foundProducts.Count);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            products = null;
            MockProductRepository = null;
        }
    }
}
