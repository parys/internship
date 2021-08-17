using System;
using System.Collections.Generic;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Question : BaseDataModel
    {
        public long QuestionNumber{ get; set; }
        public string NameQuestion { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool Deleted { get; set; }

        public Guid CreatorId { get; set; }
        public User Creator { get; set; }

        public Guid? AuditionId { get; set; }
        public Audition Audition { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<TestQuestion> TestQuestions { get; set; }
    }
}
