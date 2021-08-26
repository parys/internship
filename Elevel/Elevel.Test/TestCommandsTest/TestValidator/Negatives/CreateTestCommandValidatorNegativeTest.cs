using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Negatives
{
    public class CreateTestCommandValidatorNegativeTest
    {
        public static IEnumerable<object[]> UserIds => new List<object[]>
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
        [MemberData(nameof(UserIds))]
        public void CreateTestValidatorTest(Level level)
        {
            //Arrange

            var request = new CreateTestCommand.Request()
            {
                UserId = Guid.NewGuid(),
                Level = level
            };

            //Act

            var validator = new CreateTestCommand.Validator();
            var result = validator.Validate(request);

            //Assert

            Assert.False(result.IsValid);

        }
    }
}
