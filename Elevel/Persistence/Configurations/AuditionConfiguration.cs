using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class AuditionConfiguration
    {
        public void Configure(EntityTypeBuilder<Audition> builder)
        {
            builder.ToTable("Audition");
        }
    }
}