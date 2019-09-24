using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace VendingMachineCodeFirst
{
    class ProxyServer
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SocketCommunication socketCommunication;
        private Controller controller = new Controller();

        public ProxyServer(SocketCommunication socketCommunication)
        {
            this.socketCommunication = socketCommunication;
        }

        public void HandleCommands()
        {
            try
            {
                string message = socketCommunication.ReceiveMessage();
                string option = message.Split(' ')[0];
                string data = message.Split(' ')[1];
                switch (option)
                {
                    case AdminRequest.GET:
                        GetProducts();
                        break;
                    case AdminRequest.ADD:
                        AddProduct(data);
                        break;
                    case AdminRequest.UPDATE:
                        UpdateProduct(data);
                        break;
                    case AdminRequest.DELETE:
                        Delete(data);
                        break;
                    case AdminRequest.REFILL:
                        Refill();
                        break;
                    case AdminRequest.REPORT:
                        Report();
                        break;
                    default:
                        log.Info("Wrong admin request");
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

        private void GetProducts()
        {
            IList<Product> list = controller.GetProducts();
            socketCommunication.SendData(JsonConvert.SerializeObject(list));
        }

        private void AddProduct(string data)
        {
            Product product = JsonConvert.DeserializeObject<Product>(data);
            controller.AddProduct(product);
            log.Info(product);
            socketCommunication.SendData("Success ADD");
        }

        private void UpdateProduct(string data)
        {
            Product productToUpdate = JsonConvert.DeserializeObject<Product>(data);
            controller.UpdateProduct(productToUpdate);
            log.Info(productToUpdate);
            socketCommunication.SendData("Success UPDATE");
        }

        private void Delete(string data)
        {
            int id = JsonConvert.DeserializeObject<int>(data);
            controller.DeleteProduct(id);
            socketCommunication.SendData("Success DELETE");
        }

        private void Refill()
        {
            if (controller.Refill())
            {
                socketCommunication.SendData("Success REFILL");
            }

            socketCommunication.SendData("Fail REFILL");
        }

        private void Report()
        {
            string dataReport = controller.GenerateReport();
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
        }
    }
}
