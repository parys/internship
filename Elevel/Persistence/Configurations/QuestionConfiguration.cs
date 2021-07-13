using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class QuestionConfiguration
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question");

            builder.HasOne(x => x.Audition)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.AuditionId);
        }
    }
}