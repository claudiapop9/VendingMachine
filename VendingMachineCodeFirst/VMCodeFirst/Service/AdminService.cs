using System.Collections.Generic;

namespace VendingMachineCodeFirst.Service
{
    public class AdminService
    {
        private readonly IProductRepository productRepository = new ProductRepository();
        private readonly DataRepository dataStorage = new DataRepository();
        private readonly ReportRepository report = new ReportRepository();
        private readonly TransactionService transaction = new TransactionService();

        internal void AddProduct(Product p)
        {
            this.productRepository.AddProduct(p);
            dataStorage.PersistData(this.productRepository.GetProducts());
        }

        internal void UpdateProduct(Product p)
        {
            this.productRepository.UpdateProduct(p);
        }

        internal void DeleteProduct(int productId)
        {
            this.productRepository.RemoveProduct(productId);
            dataStorage.PersistData(this.productRepository.GetProducts());
        }

        public bool Refill()
        {
            IList<Product> productsToRefList = productRepository.GetProductsToRefill();
            if (productRepository.Refill())
            {
                dataStorage.PersistData(this.productRepository.GetProducts());
                transaction.AddTransactionRefill(productsToRefList);
                return true;
            }
            return false;
        }

        public string GenerateReport()
        {
            List<Transaction> transactions = transaction.GetTransactions();
            return report.GenerateReport(transactions);
        }

        public IList<Product> GetProducts()
        {
            return productRepository.GetProducts();
        }
    }
}
