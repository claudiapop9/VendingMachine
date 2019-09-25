using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;


namespace VendingMachineCodeFirst
{
    public class ProductRepository : IProductRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void AddProduct(Product p)
        {
            try
            {
                using (var context = new VendMachineDbContext())
                {
                    context.Products.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed-ADD");
            }
        }

        public void UpdateProduct(Product p)
        {
            try
            {
                using (var context = new VendMachineDbContext())
                {
                    context.Entry(p).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed-UPDATE");
            }
        }

        public void RemoveProduct(int productId)
        {
            try
            {
                using (var context = new VendMachineDbContext())
                {
                    Product p = context.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                    context.Products.Remove(p);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed-remove prod");
            }
        }

        public void DecreaseProductQuantity(int productId)
        {
            try
            {
                using (var context = new VendMachineDbContext())
                {
                    Product p = context.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                    p.Quantity -= 1;
                    UpdateProduct(p);
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed-DecreaseProd");
            }
        }

        public double GetProductPriceByKey(int id)
        {
            using (var context = new VendMachineDbContext())
            {
                Product p = context.Products.Where(x => x.ProductId == id).FirstOrDefault();
                double price = (Double)p.Price;
                return price;
            }
        }

        public bool Refill()
        {
            try
            {
                IList<Product> productQuantity = GetProductsToRefill();
                using (var context = new VendMachineDbContext())
                {
                    foreach (Product prod in productQuantity)
                    {
                        prod.Quantity = 10;
                        context.Entry(prod).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                log.Info("REFILL successful");

                return true;
            }
            catch (Exception)
            {
                log.Error("REFILL failed");
                return false;
            }
        }

        public IList<Product> GetProductsToRefill()
        {
            try
            {
                using (var context = new VendMachineDbContext())
                {
                    IList<Product> productQuantity = (from product in context.Products
                                                      where (product.Quantity != 10)
                                                      select product).ToList();
                    return productQuantity;
                }
            }
            catch (Exception)
            {
                log.Error("FIND refill products failed");
                return new List<Product>();
            }
        }

        public IList<Product> GetProducts()
        {
            try
            {
                using (var context = new VendMachineDbContext())
                {
                    return context.Products.ToList<Product>(); ;
                }
            }
            catch (Exception)
            {
                log.Error("Db connection-GET Prod");
                return new List<Product>();
            }
        }
    }
}
