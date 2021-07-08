using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Speaking: BaseDataModel
    {
        public Guid SpeakingId { get; set; }

        public Level Level { get; set; }

        public string Topic { get; set; }

        public DateTimeOffset CreationDate { get; set; }

    }
}
