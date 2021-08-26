using Elevel.Application.Features.TestCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Positives
{
    public class CheckTestCommandValidatorPositiveTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                0,
                10
            },
            new object[]
            {
                10,
                0
            },
            new object[]
            {
                0,
                0
            },
            new object[]
            {
                10,
                10
            },
            new object[]
            {
                5,
                5
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void CheckTestCommandValidatorPositiveTestExecution(int speakingMark, int essayMark)
        {
            //Arrange

            var request = new CheckTestCommand.Request
            {
                CoachId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                SpeakingMark = speakingMark,
                EssayMark = essayMark
            };

            //Act

            var validator = new CheckTestCommand.Validator();
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
        }
    }
}
