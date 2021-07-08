using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Audition: BaseDataModel
    {
        public Guid AuditionId { get; set; }

        public string AuditionAudio { get; set; }

        public Guid QuestionId { get; set; }

        public Level Level { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
