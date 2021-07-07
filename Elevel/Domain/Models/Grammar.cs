using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Grammar: BaseDataModel
    {
        public Guid QuestionId { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public DateTimeOffset CreationDate { get; set; }

        public Level Level { get; set; }

        public ICollection<Test> Tests { get; set; } = new List<Test>();

    }
}
