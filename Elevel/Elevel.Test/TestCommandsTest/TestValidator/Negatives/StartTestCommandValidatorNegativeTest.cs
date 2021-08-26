using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Negatives
{
    public class StartTestCommandValidatorNegativeTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                (Level)0
            },
            new object[]
            {
                (Level)6
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void StartTestCommandValidatorNegativeTestExecution(Level level)
        {
            //Arrange

            var request = new StartTestByIdQuery.Request
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Level = level
            };

            //Act

            var validator = new StartTestByIdQuery.Validator();
            var result = validator.Validate(request);

            //Assert

            Assert.False(result.IsValid);
        }
    }
}
