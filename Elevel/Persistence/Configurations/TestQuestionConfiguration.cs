using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class TestQuestionConfiguration
    {
        public void Configure(EntityTypeBuilder<TestQuestion> builder)
        {
            builder.ToTable("Test");

            builder.HasOne(x => x.UserAnswer)
                .WithMany(x => x.TestQuestions)
                .HasForeignKey(x => x.UserAnswerId);

            builder.HasOne(x => x.Test)
                .WithMany(x => x.TestQuestions)
                .HasForeignKey(x => x.TestId);

            builder.HasOne(x => x.Question)
                .WithMany(x => x.TestQuestions)
                .HasForeignKey(x => x.QuestionId);
        }
    }
}