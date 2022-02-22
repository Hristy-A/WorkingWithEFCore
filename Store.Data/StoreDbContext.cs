using Microsoft.EntityFrameworkCore;
using Store.Data.ModelConfigurations;
using Store.Data.Entities;
using Npgsql;

namespace Store.Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<User> Users { get; set; }

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

            // TODO: (done) добавить уникальный индекс на колонку Login, создать миграцию MakeLoginUnique (HasIndex(), Unique())
        }
    }
}
