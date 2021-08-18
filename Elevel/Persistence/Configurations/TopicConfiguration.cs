using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("Topics");

            builder.HasOne(x => x.Creator)
                .WithMany(x => x.Topics)
                .HasForeignKey(x => x.CreatorId);

            builder.Property(x => x.TopicNumber)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreationDate)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}