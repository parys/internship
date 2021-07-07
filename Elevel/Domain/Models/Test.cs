using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Test: BaseDataModel
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? HrId { get; set; }
        public User Hr { get; set; }

        public Guid? CoachId { get; set; }
        public User Coach { get; set; }

        public Level Level { get; set; }

        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset AssignmentStartDate { get; set; }
        public DateTimeOffset AssignmentEndDate { get; set; }

        public ICollection<Grammar> Grammars{ get; set; } = new List<Grammar>();
        public int? GrammarMark { get; set; }

        public Guid? AuditionId { get; set; }
        //public Audition Audition { get; set; }
        public int? AuditionMark { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

        public Guid? EssayId{ get; set; }
        //public Essay Essay { get; set; }
        public string EssayAnswer { get; set; }
        public int? EssayMark { get; set; }

        public Guid? SpeakingId { get; set; }
        //public Speaking Speaking { get; set; }
        public string SpeakingAnswerReference { get; set; }
        public int? SpeakingMark { get; set; }

        public string Comment { get; set; }

    }
}
