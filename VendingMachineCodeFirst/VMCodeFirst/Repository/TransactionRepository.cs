using System;
using System.Linq;
using System.Collections.Generic;

namespace VendingMachineCodeFirst
{
    public class TransactionRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void AddTransaction(Transaction t)
        {
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    db.Transactions.Add(t);
                    db.SaveChanges();
                    log.Info("Transaction Added");
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed-transactions");
            }
        }

        public List<Transaction> GetTransactions()
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();
                using (var db = new VendMachineDbContext())
                {
                    transactions = db.Transactions.ToList<Transaction>();
                }
                return transactions;

            }
            catch (Exception)
            {
                log.Error("Db connection");
                return new List<Transaction>();
            }
        }
    }
}
