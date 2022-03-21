using Microsoft.EntityFrameworkCore;

using Store.Data.ModelConfigurations;
using Store.Data.Extensions;
using Store.Data.Entities;

namespace Store.Data
{
    /// <summary>
    /// Interface for application database context
    /// </summary>
    public abstract class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public DbSet<AccountHistory> AccountHistories { get; set; }
        public DbSet<EventTypeInfo> EventTypeInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ProductEntityTypeConfiguration().Configure(modelBuilder.Entity<Product>());
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new AccountHistoryTypeConfiguration().Configure(modelBuilder.Entity<AccountHistory>());
            new EventTypeInfoTypeConfiguration().Configure(modelBuilder.Entity<EventTypeInfo>());

            InitializeHelper.InitilizeEnumTable<EventType>(modelBuilder);
        }
    }
}
