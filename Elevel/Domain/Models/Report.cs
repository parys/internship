using System;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Report:BaseDataModel
    {
        public Guid? QuestionId { get; set; }
        public Question Question { get; set; }

        public Guid? AuditionId { get; set; }
        public Audition Audition { get; set; }

        public Guid? TopicId { get; set; }
        public Topic Topic { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }

        public string Description { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}