using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities;

namespace Store.Data.ModelConfigurations
{
    internal class EventTypeInfoTypeConfiguration : IEntityTypeConfiguration<EventTypeInfo>
    {
        public void Configure(EntityTypeBuilder<EventTypeInfo> builder)
        {
            builder.HasKey(x => x.TypeId);

            builder.Property(x => x.TypeId)
                .ValueGeneratedNever();
        }
    }
}
