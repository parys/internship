using Elevel.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Elevel.Test.TopicCommand;

namespace Elevel.Test
{
    public class TopicServiceTests
    {
        [Fact]
        public void Test_Topic_Object()
        {
            //Arrange
            var topic = new Mock<Topic>();
            var command = new Mock<ITopicCommand>();
            command.Setup(x => x.GetToppic()).Returns(topic.Object);
            command.Setup(x => x.GetTopicName(It.IsAny<Guid>())).Returns(topic.Object);

            var commandObj = command.Object;
            var sut = new TopicService(commandObj);

            //Act
            var actual = sut.GetTopicFromCommand();

            //Assert
            Assert.NotNull(actual);
        }
    }
}
