using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasOne(x => x.Audition)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.AuditionId)
                .IsRequired(false);

            builder.Property(x => x.QuestionNumber)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreationDate)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.HasMany(x => x.Answers)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}