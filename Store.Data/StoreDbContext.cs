using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;

namespace Store.Data
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var csb = new Npgsql.NpgsqlConnectionStringBuilder();
            csb.Database = "store";
            csb.Host = "localhost";
            csb.Username = "postgres";
            csb.Password = "admin";

            var connectionString = csb.ToString();

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity
                    .Property(x => x.Name)
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Manufacturer)
                .WithMany()
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
