using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.ModelConfigurations
{
    public class AccountHistoryTypeConfiguration : IEntityTypeConfiguration<AccountHistory>
    {
        public void Configure(EntityTypeBuilder<AccountHistory> builder)
        {
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.AccountHistory)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
