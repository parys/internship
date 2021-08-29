using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elevel.Test.TopicCommand;

namespace Elevel.Test
{
    public class TopicService
    {
        private readonly ITopicCommand _command;
        public TopicService(ITopicCommand command)
        {
            _command = command;
        }

        public Topic GetTopicFromCommand()
        {
            return _command.GetToppic();
        }
    }
}
