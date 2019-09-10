using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachineCodeFirst;

namespace VMCodeFirstTests
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void AddProductToListTest()
        {
            Product productTest = new Product("ProductTest", 2, 8);
            Controller ctrl = new Controller();

            int initialNo = ctrl.GetProductsList().Count;
            ctrl.AddProductToList(productTest);
            int updatedNo = ctrl.GetProductsList().Count;

            Assert.AreEqual(initialNo + 1, updatedNo);
        }
    }
}
