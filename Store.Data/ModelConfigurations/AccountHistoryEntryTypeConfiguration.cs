using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.ModelConfigurations
{
    public class AccountHistoryEntryTypeConfiguration : IEntityTypeConfiguration<AccountHistoryEntry>
    {
        public void Configure(EntityTypeBuilder<AccountHistoryEntry> builder)
        {
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.AccountHistory)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.
                ToTable("accountHistory");
        }
    }
}
