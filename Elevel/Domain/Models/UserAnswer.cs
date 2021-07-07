using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class UserAnswer : BaseDataModel
    {
        public Guid UserAnswerId { get; set; }
        public Question Question { get; set; }
        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }
        public Guid TestId { get; set; }
    }
}
