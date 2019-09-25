using VendingMachineCodeFirst.Repository;

namespace VendingMachineCodeFirst.Service
{
    class CardPayment : IPayment
    {
        private ICardRepository cardRepo;
        public CardPayment(string cardNo, string pin) => this.cardRepo=new CardRepository(cardNo, pin); 
        public CardPayment(ICardRepository cardRepository)
        {
            this.cardRepo = cardRepository;
        }

        public void Pay(double cost)
        {
            cardRepo.UpdateAmount(cost);
        }

        public bool IsEnough(double cost)
        {
            return cardRepo.IsEnough(cost);
        }

        public bool IsValid()
        {
            return cardRepo.IsValidCard();
        }
    }
}
