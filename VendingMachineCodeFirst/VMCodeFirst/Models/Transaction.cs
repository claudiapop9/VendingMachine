using System;


namespace VendingMachineCodeFirst
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Type{ get; set; }
        public DateTime? Date { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Transaction()
        {}
        
        public Transaction(string type, int productId)
        {
            Type = type;
            Date = DateTime.Now;
            ProductId = productId;
        }
    }
}
