namespace VendingMachineCodeFirst
{
    public interface ICashMoneyRepository
    {
        void UpdateMoney(double value, int quantity);
        void GiveChange(double change);
    }
}
