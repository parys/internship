using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Elevel.Domain.Models
{
    public class Test: BaseDataModel
    {
        public Level Level { get; set; }

        public long TestNumber { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? TestPassingDate { get; set; }
        public DateTimeOffset? AssignmentEndDate { get; set; }

        public bool Priority { get; set; }

        public int? GrammarMark { get; set; }
        public int? AuditionMark { get; set; }
        public int? EssayMark { get; set; }
        public int? SpeakingMark { get; set; }

        public string EssayAnswer { get; set; }
        public string SpeakingAnswerReference { get; set; }
        public string Comment { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? HrId { get; set; }
        public User Hr { get; set; }

        public Guid? CoachId { get; set; }
        public User Coach { get; set; }

        public Guid? AuditionId { get; set; }
        public Audition Audition { get; set; }

        public Guid? EssayId { get; set; }
        public Topic Essay { get; set; }

        public Guid? SpeakingId { get; set; }
        public Topic Speaking { get; set; }

        public ICollection<TestQuestion> TestQuestions { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
