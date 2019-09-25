using System;
using System.Linq;
using System.Data.Entity;

namespace VendingMachineCodeFirst.Repository
{
    class CardRepository : ICardRepository
    {
        private string cardNo;
        private string pin;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CardRepository(){}

        public CardRepository(string cardNo, string pin)
        {
            this.cardNo = cardNo;
            this.pin = pin;
        }

        public void UpdateAmount(double cost)
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
            catch (Exception)
            {
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
                    return false;
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed");
                return false;
            }
        }

        public bool IsValidCard()
        {
            try
            {
                using (var db = new VendMachineDbContext())
                {
                    Account Account = db.Accounts.Where(x => x.CardNO == cardNo).FirstOrDefault();
                    if (Account != null && Account.Pin == pin)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                log.Error("Db connection failed");
                return false;
            }
        }
    }
}
