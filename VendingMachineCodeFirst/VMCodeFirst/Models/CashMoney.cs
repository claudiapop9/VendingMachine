namespace VendingMachineCodeFirst
{
        
    public class CashMoney
    {
        public int Id { get; set; }
        public double MoneyValue { get; set; }
        public int Quantity { get; set; }

        public CashMoney() { }

        public CashMoney(double moneyValue, int quantity)
        {
            MoneyValue = moneyValue;
            Quantity = quantity;
        }
    }
}
