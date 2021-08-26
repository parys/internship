using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elevel.Test.TopicCommand;

namespace Elevel.Test
{
    public class TopicCommand : ITopicCommand
    {
        private readonly Topic _topic;
        private readonly List<Topic> _topics; 

        public TopicCommand(Topic topic)
        {
            _topic = topic;
            
        }

        public Topic GetTopic()
        {
            return _topic;
        }

        public Topic GetTopicName(Guid id)
        {
            return _topics.SingleOrDefault(x => x.Id == id);
        }

        public Topic GetToppic()
        {
            return _topic;
        }

        public interface ITopicCommand
        {
            Topic GetToppic();
            Topic GetTopicName(Guid id);
        }
    }
}
