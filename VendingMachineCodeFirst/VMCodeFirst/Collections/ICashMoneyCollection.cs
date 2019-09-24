namespace VendingMachineCodeFirst
{
    public interface ICashMoneyCollection
    {
        void UpdateMoney(double value, int quantity);
        void GiveChange(double change);
    }
}
