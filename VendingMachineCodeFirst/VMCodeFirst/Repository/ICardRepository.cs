namespace VendingMachineCodeFirst.Repository
{
    public interface ICardRepository
    {
        void UpdateAmount(double cost);
        bool IsEnough(double cost);
        bool IsValidCard();
    }
}
