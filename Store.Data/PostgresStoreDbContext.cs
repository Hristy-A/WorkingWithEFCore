using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Store.Data
{
    public class PostgresStoreDbContext : StoreDbContext
    {
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
    }
}
