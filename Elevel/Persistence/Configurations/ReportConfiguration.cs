using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elevel.Infrastructure.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports");

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserReports)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Question)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.QuestionId)
                .IsRequired(false);

            builder.HasOne(x => x.Audition)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.AuditionId);

            builder.HasOne(x => x.Topic)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.TopicId)
                .IsRequired(false);

            builder.HasOne(x => x.Test)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.TestId)
                .IsRequired(false);

            builder.Property(x => x.ReportStatus).HasDefaultValue(ReportStatus.Created);

            builder.Property(x => x.CreationDate)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}