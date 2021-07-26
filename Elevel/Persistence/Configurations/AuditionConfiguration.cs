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

            builder.Property(x => x.CreationDate).HasDefaultValueSql("GETUTCDATE()");

            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}