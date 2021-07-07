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
        public Question<Grammar> Question { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public string Level { get; set; }

        public ICollection<Test> Tests { get; set; }

        public Grammar()
        {
            Tests = new List<Test>();
        }
    }
}
