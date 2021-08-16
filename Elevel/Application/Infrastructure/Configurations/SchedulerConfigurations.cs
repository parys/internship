using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Infrastructure.Configurations
{
    public class SchedulerConfigurations
    {
        public int startHour { get; set; }
        public int startMinute { get; set; }
        public int startSecond { get; set; }
        public int startDay { get; set; }
        public int startMonth { get; set; }
    }
}
