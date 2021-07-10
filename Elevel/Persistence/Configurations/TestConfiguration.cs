using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("Test");

            builder.HasOne(x => x.Essay)
                .WithMany(x => x.EssayTests)
                .HasForeignKey(x => x.EssayId);

            builder.HasOne(x => x.Speaking)
                .WithMany(x => x.SpeakingTests)
                .HasForeignKey(x => x.SpeakingId);
        }
    }
}