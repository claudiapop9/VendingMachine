namespace VendingMachineCodeFirst.Migrations
{
    using System.Data.Entity.Migrations;
    using VendingMachineCommon;


    internal sealed class Configuration : DbMigrationsConfiguration<VendMachineDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VendingMachineCodeFirst.VendMachineDbContext context)
        {
            context.Money.AddOrUpdate(x => x.Id,
            new CashMoney() { Id = 1, MoneyValue = 10, Quantity = 10 },
            new CashMoney() { Id = 2, MoneyValue = 5, Quantity = 10 },
            new CashMoney() { Id = 3, MoneyValue = 1, Quantity = 10 },
            new CashMoney() { Id = 4, MoneyValue = 0.5, Quantity = 10 });

            context.Accounts.AddOrUpdate(x => x.AccountId,
            new Account() { AccountId = 1, CardNO = "1234", Pin = "1004", Amount = 100 },
            new Account() { AccountId = 2, CardNO = "5678", Pin = "5555", Amount = 50 },
            new Account() { AccountId = 3, CardNO = "4321", Pin = "1234", Amount = 10 });
            
            context.Products.AddOrUpdate(x => x.ProductId,
            new Product() { ProductId = 1, Name = "Coca Cola", Quantity = 5, Price = 3.5 },
            new Product() { ProductId = 2, Name = "Kinder Bueno", Quantity = 5, Price = 3.5 },
            new Product() { ProductId = 3, Name = "Bakerolls", Quantity = 7, Price = 5 },
            new Product() { ProductId = 4, Name = "Sprite", Quantity = 5, Price = 3.5 },
            new Product() { ProductId = 5, Name = "Almonds", Quantity = 5, Price = 2 });
        }
    }
}
