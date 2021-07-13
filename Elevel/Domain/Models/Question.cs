using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Question : BaseDataModel
    {
        public string QuestionName { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Guid AnswerId { get; set; }
        public bool Deleted { get; set; }
        public Guid? AuditionId { get; set; }
        public Audition Audition { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<TestQuestion> TestQuestions { get; set; }
    }
}
