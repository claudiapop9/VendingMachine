using System;
using System.Collections.Generic;
using VendingMachineCodeFirst.Service;

namespace VendingMachineCodeFirst
{
    public class ClientController
    {
        private ClientService service;
        private Validator validator = new Validator();

        public ClientController(ClientService service)
        {
            this.service = service;
        }

        public bool BuyProduct(string id)
        {
            if (validator.isIdValid(id))
            {
                int productId = Int32.Parse(id);
                return service.BuyProduct(productId);
            }
            return false;
        }

        public IList<Product> GetProducts()
        {
            return service.GetProducts();
        }
    }
}