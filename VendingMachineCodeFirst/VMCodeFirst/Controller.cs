using System.Collections.Generic;


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

        public Controller(){}

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

        public IList<Product> GetProducts()
        {
            return productCollection.GetProducts();
        }

        public void AddProduct(Product p)
        {
            this.productCollection.AddProduct(p);
            dataStorage.PersistData(this.productCollection.GetProducts());
        }

        public void UpdateProduct(Product p)
        {
            this.productCollection.UpdateProduct(p);
        }

        public void DeleteProduct(int productId)
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

            return false;
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