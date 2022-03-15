using Microsoft.EntityFrameworkCore;

using Store.Data.ModelConfigurations;
using Store.Data.Extensions;
using Store.Data.Entities;

using Npgsql;

namespace Store.Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AccountHistory> AccountHistories { get; set; }
        public DbSet<EventTypeInfo> EventTypeInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var csb = new NpgsqlConnectionStringBuilder
            {
                Database = "store",
                Host = "localhost",
                Username = "postgres",
                Password = "admin"
            };

            var connectionString = csb.ToString();

            optionsBuilder
                .UseNpgsql(connectionString)
                .UseCamelCaseNamingConvention();
        }

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
