using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test
{
    public class TopicCommandTest
    {
        [Fact]
        public void Topic_Test()
        {
            //Arrange
            var topic = GetTopic();
            var sut = new TopicCommand(topic);
            //Act

            //Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public void Test_Topic_Object()
        {
            //Arrange
            var expected = GetTopic();
            var sut = new TopicCommand(expected);
            //Act
            var actual = sut.GetTopic();
            //Assert
            Assert.Equal(expected, actual);
        }

        private Topic GetTopic()
        {
            return new Topic
            {
               
            };
        }
    }
}
