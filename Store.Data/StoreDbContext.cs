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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AccountHistoryEntry> AccountHistory { get; set; }
        public DbSet<EventTypeInfo> EventTypeInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new AccountHistoryEntryTypeConfiguration().Configure(modelBuilder.Entity<AccountHistoryEntry>());
            new EventTypeInfoTypeConfiguration().Configure(modelBuilder.Entity<EventTypeInfo>());

            InitializeHelper.InitializeEnumTable<EventType>(modelBuilder);
        }
    }
}
