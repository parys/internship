using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class AuditionConfiguration : IEntityTypeConfiguration<Audition>
    {
        public void Configure(EntityTypeBuilder<Audition> builder)
        {
            builder.ToTable("Auditions");

            builder.Property(x => x.CreationDate)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(x => x.AuditionNumber)
                .ValueGeneratedOnAdd();

            builder.HasQueryFilter(x => !x.Deleted);

            builder.HasOne(x => x.Creator)
                .WithMany(x => x.Auditions)
                .HasForeignKey(x => x.CreatorId);
        }
    }
}