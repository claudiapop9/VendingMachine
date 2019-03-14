﻿using System.Data.Entity;


namespace VendingMachineCodeFirst
{
    class VendMachineDbContext:DbContext
    {
        public VendMachineDbContext(): base("name=VendMachineDbContext")
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CashMoney> Money { get; set; }
    }
}
