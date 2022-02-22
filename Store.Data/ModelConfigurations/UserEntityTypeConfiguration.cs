using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.ModelConfigurations
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(x => x.Roles)
                .WithMany(x => x.Users);

            builder
                .Property(x => x.Login)
                .IsRequired();

            builder
                .HasIndex(x => x.Login)
                .IsUnique();
        }
    }
}
