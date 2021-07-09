using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elevel.Domain.Enums;

namespace Elevel.Domain.Models
{
    public class Audition : BaseDataModel
    {
        public string AuditionAudio { get; set; }
        public Guid QuestionId { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}