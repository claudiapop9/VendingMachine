﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VendingMachineAdmin
{
    class Controller
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AdminSocket admin = new AdminSocket();
        private List<Product> products;

        public List<Product> GetAllProduct()
        {
            try
            {
                string data = admin.Get("GET Products");
                try
                {
                    products = JsonConvert.DeserializeObject<List<Product>>(data);
                    admin.ReleaseSocket();
                    return products;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    log.Error("Json convert" + ex);
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                log.Error("GET Products" + e);
            }

            return products;
        }

        public string AddProductToList(string tupleItem1, int tupleItem2, double tupleItem3)
        {
            try
            {
                Product product = new Product(tupleItem1, tupleItem2, tupleItem3);
                admin.SendMessage("ADD " + JsonConvert.SerializeObject(product));
                string message = admin.ReceiveMessage();
                admin.ReleaseSocket();
                log.Info("ADD "+ product);
                return message;
            }
            catch (Exception)
            {
                log.Error("Add Product");
            }

            return "ERROR";
        }

        public string UpdateProductInList(int id, string tupleItem1, int tupleItem2, double tupleItem3)
        {
            try
            {
                Product product = new Product(id, tupleItem1, tupleItem2, tupleItem3);
                admin.SendMessage("UPDATE " + JsonConvert.SerializeObject(product));
                string message = admin.ReceiveMessage();
                admin.ReleaseSocket();
                log.Info("UPDATE " + product);
                return message;
            }
            catch (Exception)
            {
                log.Error("Update Product");
            }

            return "ERROR";
        }

        public string DeleteProductFromList(int id)
        {
            admin.SendMessage("DELETE " + JsonConvert.SerializeObject(id));
            string message = admin.ReceiveMessage();
            admin.ReleaseSocket();
            log.Info("DELETE "+ id);
            return message;
        }

        public string Refill()
        {
            admin.SendMessage("REFILL ");
            string message = admin.ReceiveMessage();
            admin.ReleaseSocket();
            log.Info("REFILL");
            return message;
        }

        public string GenerateReport()
        {
            admin.SendMessage("REPORT ");
            string message = admin.ReceiveMessage();
            admin.ReleaseSocket();
            log.Info("REPORT");
            return message;
        }
    }
}