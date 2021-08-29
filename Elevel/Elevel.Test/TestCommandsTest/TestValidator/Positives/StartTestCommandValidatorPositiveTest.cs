using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Positives
{
    public class StartTestCommandValidatorPositiveTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                Level.Elementary
            },
            new object[]
            {
                Level.PreIntermediate
            },
            new object[]
            {
                 Level.Intermediate
            },
            new object[]
            {
                 Level.Upperintermediate
            },
            new object[]
            {
                Level.Advanced
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void StartTestCommandValidatorPositiveTestExecution(Level level)
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

            Assert.True(result.IsValid);
        }
    }
}
