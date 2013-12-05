using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using HQServer.Domain.Entities;


namespace HQServer.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<BatchResponse> BatchResponses { get; set; }
        public DbSet<BatchResponseDetail> BatchResponseDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<OutletInventory> OutletInventories { get; set; }
        public DbSet<OutletTransaction> OutletTransactions { get; set; }
        public DbSet<OutletTransactionDetail> OutletTransactionDetails { get; set; }
        public DbSet<OnlineTransaction> Transactions { get; set; }
        public DbSet<OnlineTransactionDetail> TransactionDetails { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
