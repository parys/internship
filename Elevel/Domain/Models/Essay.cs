using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Domain.Models
{
    public class Essay: BaseDataModel
    {
        public Guid EssayId { get; set; }

        public LevelEnum Level { get; set; }

        public string Topic { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
