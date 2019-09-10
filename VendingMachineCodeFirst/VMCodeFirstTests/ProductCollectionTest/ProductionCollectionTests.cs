using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VendingMachineCodeFirst;
using VMCodeFirstTests.ProductCollectionTest;

namespace VMCodeFirstTests
{
    [TestClass]
    public class ProductionCollectionTests
    {
        IProductCollection service;
        IList<Product> data;

        [TestInitialize]
        public void TestInit()
        {

            IList<Product> data = new List<Product> {
                new Product { Name = "ProdTest1", Quantity=3, Price=10},
                new Product { Name = "ProdTest2", Quantity=10, Price=8 },
                new Product { Name = "ProdTest3", Quantity=3, Price=5 },
            };

            Mock<IProductCollection> MockProductionCollection = new Mock<IProductCollection>();
            this.service = MockProductionCollection.Object;
        }

        [TestMethod]
        public void UpdateProduct()
        {
            Product p = new Product { ProductId = 0, Name = "ProdTest1a", Quantity = 4, Price = 10 };
            service.UpdateProduct(p);
            IList<Product> products = service.GetProducts();
            Assert.AreEqual("ProdTest1a", products[0].Name);
        }

        [TestMethod]
        public void DecreaseProductQuantityWhenIdExists()
        {

            service.DecreaseProductQuantity(0);
            IList<Product> products = service.GetProducts();
            Assert.AreEqual(2, products[0].Quantity);
        }


        [TestMethod]
        public void RemoveProductWhenIdExists()
        {

            service.RemoveProduct(0);
            IList<Product> products = service.GetProducts();

            Assert.AreEqual(2, products.Count);
            //Assert.AreEqual("ProdTest1", products[0].Name);
            Assert.AreEqual("ProdTest2", products[0].Name);
            Assert.AreEqual("ProdTest3", products[1].Name);
        }

        [TestMethod]
        public void GetProductPriceByKeyWhenProductExists()
        {
            double price = service.GetProductPriceByKey(0);

            Assert.AreEqual(10, price);
        }

        [TestMethod]
        public void GetProductPriceByKeyWhenNoProduct()
        {
            double price = service.GetProductPriceByKey(5);

            Assert.AreEqual(-1, price);
        }

        //[TestMethod]
        //public void RefillWhenQuantity10ReturnTrue()
        //{

        //    Assert.AreEqual(true, service.Refill());
        //}

        [TestMethod]
        public void GetProductsToRefillWhenProductsWithQuantityLessThan10ReturnsProductsList()
        {
            IList<Product> products = service.GetProductsToRefill();
            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("ProdTest1", products[0].Name);
            Assert.AreEqual("ProdTest3", products[1].Name);
        }

        [TestMethod]
        public void GetProductsToRefillWhenProductsWithNoQuantityLessThan10ReturnsEmptyList()
        {
            IQueryable<Product> data = new List<Product>
            {
                new Product { Name = "ProdTest1", Quantity=10, Price=10},
                new Product { Name = "ProdTest2", Quantity=10, Price=10 },
                new Product { Name = "ProdTest3", Quantity=10, Price=10 },
            }.AsQueryable();

            Mock<DbSet<Product>> mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Mock<VendMachineDbContext> mockContext = new Mock<VendMachineDbContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);
            service = new ProductCollection(mockContext.Object);

            IList<Product> products = service.GetProductsToRefill();
            Assert.AreEqual(0, products.Count);
        }

        [TestMethod]
        public void GetAllProducts()
        {
            IList<Product> products = service.GetProducts();

            Assert.AreEqual(3, products.Count);
            Assert.AreEqual("ProdTest1", products[0].Name);
            Assert.AreEqual("ProdTest2", products[1].Name);
            Assert.AreEqual("ProdTest3", products[2].Name);
        }


        [TestCleanup]
        public void TestCleanUp()
        {
            service = null;
        }
    }
}
