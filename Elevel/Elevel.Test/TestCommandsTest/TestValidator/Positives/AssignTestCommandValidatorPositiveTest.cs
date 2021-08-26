using Elevel.Application.Features.TestCommands;
using System;
using System.Collections.Generic;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Positives
{
    public class AssignTestCommandValidatorPositiveTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                DateTimeOffset.UtcNow,
                Guid.NewGuid()
            },
            new object[]
            {
                DateTimeOffset.UtcNow.AddDays(1),
                Guid.NewGuid()
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void AssignTestValidatorpositiveTestExecution(DateTimeOffset assignmentEndDate, Guid userId)
        {
            // Arrange

            var request = new AssignTestCommand.Request
            {
                AssignmentEndDate = assignmentEndDate,
                UserId = userId,
                HrId = Guid.NewGuid()
            };

            //Act

            var validator = new AssignTestCommand.Validator();
            var result = validator.Validate(request);

            //Assert

            Assert.True(result.IsValid);
        }
    }
}
