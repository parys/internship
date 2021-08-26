using Elevel.Application.Features.TestCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Positives
{
    public class SubmitTestCommandValidatorPositiveTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                new string('c',512)
            },
            new object[]
            {
                new List<Guid>
                {

                },
                new List<Guid>
                {

                },
                string.Empty
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void SubmitTestCommandValidatorPositiveTestExecution(IEnumerable<Guid> grammarAnswers, IEnumerable<Guid> auditionAnswers, string essayAnswer)
        {
            //Arrange

            var request = new SubmitTestCommand.Request
            {
                AuditionAnswers = auditionAnswers,
                GrammarAnswers = grammarAnswers,
                EssayAnswer = essayAnswer,
                SpeakingAnswerReference = String.Empty,
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            //Act

            var validator = new SubmitTestCommand.Validator();
            var result = validator.Validate(request);

            //Assert

            Assert.True(result.IsValid);
        }
    }
}
