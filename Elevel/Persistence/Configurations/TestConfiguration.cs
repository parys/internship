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

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserTests)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Hr)
                .WithMany(x => x.HrTests)
                .HasForeignKey(x => x.HrId);

            builder.HasOne(x => x.Coach)
                .WithMany(x => x.CoachTests)
                .HasForeignKey(x => x.CoachId);

            builder.HasOne(x => x.Audition)
                .WithMany(x => x.Tests)
                .HasForeignKey(x => x.AuditionId);

            builder.HasMany(x => x.TestQuestions)
                .WithOne(x => x.Test);
                
        }
    }
}