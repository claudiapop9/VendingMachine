using System.Collections.Generic;
using VendingMachineCommon;


namespace VendingMachineCodeFirst.Service
{
    public class ClientService
    {
        private readonly IProductRepository productRepository = new ProductRepository();
        private DataRepository dataStorage = new DataRepository();
        private TransactionService transactionService = new TransactionService();
        private IPayment payment;

        public ClientService() { }

        public ClientService(IPayment paymentMethod, IProductRepository productRepository)
        {
            this.payment = paymentMethod;
            this.productRepository = productRepository;
        }
        public ClientService(IPayment paymentMethod)
        {
            this.payment = paymentMethod;
        }

        public bool BuyProduct(int productId)
        {
            double productPrice = productRepository.GetProductPriceByKey(productId);
            if (payment.IsValid() && payment.IsEnough(productPrice))
            {
                productRepository.DecreaseProductQuantity(productId);
                payment.Pay(productPrice);
                dataStorage.PersistData(GetProducts());
                transactionService.AddTransition("BUY", productId);
                return true;
            }
            return false;
        }

        public IList<Product> GetProducts()
        {
            return productRepository.GetProducts();
        }
    }      
}
