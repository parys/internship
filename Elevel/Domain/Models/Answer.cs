using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Answer : BaseDataModel
    {
        public string NameAnswer { get; set; }
        public bool IsRight { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public ICollection<TestQuestion> TestQuestions { get; set; }
    }
}
