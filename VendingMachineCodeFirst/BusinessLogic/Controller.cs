using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using VendingMachineCodeFirst;


namespace BusinessLogic
{
    public class Controller
    {
        private Repository repository=new Repository();
        private PaymentDTO paymentDto=new PaymentDTO();
        private ProductDTO productDto=new ProductDTO();
        private IPayment payment;
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Controller()
        {}

        public void GetPayment(string json)
        {
            this.payment = paymentDto.FromMap(json);
        }

        public string GetProducts()
        {
            return productDto.ListToMap(repository.GetProductsList());
        }

        public bool BuyProduct(int productId,string json)
        {
           GetPayment(json);
           return repository.BuyProduct(productId);
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
                            List<Product> list = repository.GetProductsList();
                            socketCommunication.SendData(JsonConvert.SerializeObject(list));
                            break;
                        case "ADD":
                            Product product = JsonConvert.DeserializeObject<Product>(data);
                            repository.AddProductToList(product);
                            log.Info(product);
                            socketCommunication.SendData("Success ADD");
                            break;
                        case "UPDATE":

                            Product productToUpdate = JsonConvert.DeserializeObject<Product>(data);
                            repository.UpdateProductInList(productToUpdate);
                            log.Info(productToUpdate);
                            socketCommunication.SendData("Success UPDATE");

                            break;
                        case "DELETE":

                            int id = JsonConvert.DeserializeObject<int>(data);
                            repository.DeleteProductFromList(id);
                            socketCommunication.SendData("Success DELETE");
                            break;
                        case "REFILL":
                            if (repository.Refill())
                            {
                                socketCommunication.SendData("Success REFILL");
                            }
                            else
                            {
                                socketCommunication.SendData("Fail REFILL");
                            }

                            break;
                        case "REPORT":
                            string dataReport = repository.GenerateReport();
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
    }
}
