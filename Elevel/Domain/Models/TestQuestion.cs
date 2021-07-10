using System;

namespace Elevel.Domain.Models
{
    public class TestQuestion : BaseDataModel
    {
        public Guid? UserAnswerId { get; set; }
        public Answer UserAnswer { get; set; }

        public Guid TestId { get; set; }
        public Test Test { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}