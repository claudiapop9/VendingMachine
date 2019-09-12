using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VendingMachineCodeFirst;

namespace BusinessLogic
{
    class ProductDTO
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ListToMap(List<Product> productsList)
        {
            List<dynamic> itemsList=new List<dynamic>();
            foreach (var product in productsList)
            {
                var productLessInfo = new
                {
                    ProductId = product.ProductId,
                    Name=product.Name,
                    Quantity=product.Quantity,
                    Price=product.Price
                };
                itemsList.Add(productLessInfo);
            }
            string json=null;
            try
            {
                json = JsonConvert.SerializeObject(itemsList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return json;
        }

        public Product FromMap(string json)
        {
            Product product = null;
            try
            {
                product = JsonConvert.DeserializeObject<Product>(json);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return product;
        }
    }
}
