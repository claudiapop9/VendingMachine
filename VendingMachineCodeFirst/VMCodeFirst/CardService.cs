using System.Data.Entity;
using System.Linq;
using System;

namespace VendingMachineCodeFirst
{
    class CardService : IPayment
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string cardNo;
        private string pin;

        public CardService(string cardNo, string pin)
        {
            this.cardNo = cardNo;
            this.pin = pin;
        }

        public void Pay(double cost)
        {
           
                try
                {
                    using (var db = new VendMachineDbContext())
                    {
                        Account Account = db.Accounts.Where(x => x.CardNO == cardNo).FirstOrDefault();
                        Account.Amount -= cost;
                        db.Entry(Account).State = EntityState.Modified;
                        db.SaveChanges();
                        log.Info("Payment success");
                    }
                }
                catch(Exception) {
                    log.Error("Db connection failed");
                }
           
        }
        public bool IsEnough(double cost)
        {
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    Account Account = db.Accounts.Where(x => x.CardNO == cardNo).FirstOrDefault();
                    if (Account.Amount >= cost)
                    {
                        
                        return true;
                    }
                   
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed");

            }
            return false;
        }
        
        private bool IsValidCard(string cardNumber, string cardPin)
        {
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    Account Account = db.Accounts.Where(x => x.CardNO == cardNumber).FirstOrDefault();
                    if (Account != null && Account.Pin == cardPin)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception) {
                log.Error("Db connection failed");
            }
            return false;
        }
    }
}
