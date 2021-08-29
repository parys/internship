using Elevel.Application.Features.TestCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Negatives
{
    public class AssignTestCommandValidatorNegativeTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                null,
                Guid.NewGuid()
            },
            new object[]
            {
                DateTimeOffset.UtcNow.AddDays(-1),
                Guid.NewGuid()
            },
            new object[]
            {
                DateTimeOffset.UtcNow,
                null
            },
            new object[]
            {
                null,
                null
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void AssignTestValidatorNegativeTestExecution(DateTimeOffset assignmentEndDate, Guid userId)
        {
            //Arrange

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

            Assert.False(result.IsValid);
        }
    }
}
