using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;


namespace VendingMachineCodeFirst
{
    public class Controller
    {
        private const string filePath = @".\currentDb.txt";
        private const string filePathAllStates = @".\all.txt";
        private const string reportPath = @".\report.txt";

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
        private readonly ProductCollection productCollection = new ProductCollection(new VendMachineDbContext());
        private Data dataStorage = new Data(filePath, filePathAllStates);
        private TransactionManager transactionManager = new TransactionManager();
        private Report report = new Report();
        private IPayment payment;


        public Controller()
        {
        }

        public Controller(IPayment paymentMethod)
        {
            this.payment = paymentMethod;
        }

        public bool BuyProduct(int productId)
        {
            double productPrice = productCollection.GetProductPriceByKey(productId);
            if (productPrice != -1 && payment.IsEnough(productPrice))
            {
                productCollection.DecreaseProductQuantity(productId);
                payment.Pay(productPrice);
                dataStorage.PersistData(this.productCollection.GetProducts());
                AddTransition("BUY", productId);
                return true;
            }

            return false;
        }

        public void Communicate()
        {
            SocketCommunication socketCommunication = new SocketCommunication();
            if (socketCommunication.IsConnected())
            {
                try
                {
                    string message = socketCommunication.ReceiveMessage();
                    string option = message.Split(' ')[0];
                    string data = message.Split(' ')[1];
                    switch (option)
                    {
                        case "GET":
                            IList<Product> list = GetProductsList();
                            socketCommunication.SendData(JsonConvert.SerializeObject(list));
                            break;
                        case "ADD":
                            Product product = JsonConvert.DeserializeObject<Product>(data);
                            AddProductToList(product);
                            log.Info(product);
                            socketCommunication.SendData("Success ADD");
                            break;
                        case "UPDATE":

                            Product productToUpdate = JsonConvert.DeserializeObject<Product>(data);
                            UpdateProductInList(productToUpdate);
                            log.Info(productToUpdate);
                            socketCommunication.SendData("Success UPDATE");

                            break;
                        case "DELETE":

                            int id = JsonConvert.DeserializeObject<int>(data);
                            DeleteProductFromList(id);
                            socketCommunication.SendData("Success DELETE");
                            break;
                        case "REFILL":
                            if (Refill())
                            {
                                socketCommunication.SendData("Success REFILL");
                            }
                            else
                            {
                                socketCommunication.SendData("Fail REFILL");
                            }

                            break;
                        case "REPORT":
                            string dataReport = GenerateReport();
                            if (dataReport != null)
                            {
                                socketCommunication.SendData(dataReport);
                                log.Info("REPORT sent");
                            }
                            else
                            {
                                socketCommunication.SendData("Generate Report fail");
                                log.Info("REPORT fail");
                            }

                            break;
                    }
                }
                catch (ArgumentNullException ane)
                {
                    log.Error("ArgumentNullException : {0}", ane);
                }
                catch (SocketException se)
                {
                    log.Error("SocketException : {0}", se);
                }
                catch (JsonException ex)
                {
                    log.Error("Json convert" + ex);
                }
                catch (Exception ex)
                {
                    log.Info(ex);
                }
            }
        }

        public IList<Product> GetProductsList()
        {
            return productCollection.GetProducts();
        }

        public void AddProductToList(Product p)
        {
            this.productCollection.AddProduct(p);
            dataStorage.PersistData(this.productCollection.GetProducts());
        }

        public void UpdateProductInList(Product p)
        {
            this.productCollection.UpdateProduct(p);
        }

        public void DeleteProductFromList(int productId)
        {
            this.productCollection.RemoveProduct(productId);
            dataStorage.PersistData(this.productCollection.GetProducts());
        }

        public bool Refill()
        {
            IList<Product> productsToRefList = productCollection.GetProductsToRefill();
            if (productCollection.Refill())
            {
                dataStorage.PersistData(this.productCollection.GetProducts());
                AddTransactionRefill(productsToRefList);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddTransactionRefill(IList<Product> products)
        {
            foreach (var prod in products)
            {
                AddTransition("REFILL", prod.ProductId);
            }
        }

        public void AddTransition(string info, int productId)
        {
            Transaction transaction = new Transaction(info, productId);
            transactionManager.AddTransaction(transaction);
        }

        public string GenerateReport()
        {
            List<Transaction> transactions = transactionManager.GetTransactions();
            return report.GenerateReport(reportPath, transactions);
        }
    }
}