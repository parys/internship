using Elevel.Application.Features.TestCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elevel.Test.TestCommandsTest.TestValidator.Negatives
{
    public class SubmitTestCommandValidatorNegativeTest
    {
        public static IEnumerable<object[]> validatorData = new List<object[]>
        {
            new object[]
            {
                Enumerable.Repeat(Guid.NewGuid(), 12),
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
                Enumerable.Repeat(Guid.NewGuid(), 12),
                new string('c',512)
            },
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
                new string('c',513)
            },
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
                new string('c',513)
            },
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
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                new string('c',513)
            }
        };

        [Theory]
        [MemberData(nameof(validatorData))]
        public void SubmitTestCommandValidatorNegativeTestExecution(IEnumerable<Guid> grammarAnswers, IEnumerable<Guid> auditionAnswers, string essayAnswer)
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

            Assert.False(result.IsValid);
        }
    }
}
