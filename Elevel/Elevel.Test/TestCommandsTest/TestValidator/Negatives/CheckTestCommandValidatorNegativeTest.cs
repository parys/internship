using Elevel.Application.Features.TestCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Negatives
{
    public class CheckTestCommandValidatorNegativeTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                -1,
                -1
            },
            new object[]
            {
                11,
                11
            },
            new object[]
            {
                -1,
                11
            },
            new object[]
            {
                11,
                -1
            },
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void CheckTestCommandValidatorNegativeTestExecution(int speakingMark, int essayMark)
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
            Assert.False(result.IsValid);
        }
    }
}
