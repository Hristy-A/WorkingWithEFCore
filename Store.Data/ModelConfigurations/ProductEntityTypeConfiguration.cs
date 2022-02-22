using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.ModelConfigurations
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(x => x.Name)
                .HasMaxLength(255);

            builder
                .HasOne(x => x.Manufacturer)
                .WithMany()
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
