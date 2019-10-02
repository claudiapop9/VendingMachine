using System.Collections.Generic;
using VendingMachineCommon;

namespace VendingMachineCodeFirst.Service
{
    class TransactionService
    {
        private TransactionRepository transactionRepository = new TransactionRepository();

        internal void AddTransactionRefill(IList<Product> products)
        {
            foreach (var prod in products)
            {
                AddTransition("REFILL", prod.ProductId);
            }
        }

        internal void AddTransition(string info, int productId)
        {
            Transaction transaction = new Transaction(info, productId);
            transactionRepository.AddTransaction(transaction);
        }

        internal List<Transaction> GetTransactions()
        {
            return transactionRepository.GetTransactions();
        }
    }
}
