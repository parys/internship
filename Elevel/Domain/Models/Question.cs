using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Question : BaseDataModel
    {
        public Guid PhaseId { get; set; }
        public string NameQuestion { get; set; }
        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}
