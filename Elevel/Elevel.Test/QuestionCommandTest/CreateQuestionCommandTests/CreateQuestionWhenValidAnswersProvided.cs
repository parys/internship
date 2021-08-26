using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using AnswerDto = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.AnswerDto;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenValidAnswersProvided : CreateQuestionValidator
    {
        public static IEnumerable<object[]> ValidData => new List<object[]>
        {
            new object[] {new List<string> {"Nitwit", "Blubber", "Oddment", "Tweat"}, new List<bool> { false, true, false, false} },
            new object[] {new List<string> {"Fall", "Winter", "Spring", "Summer"}, new List<bool> { false, false, false, true}},
            new object[] {new List<string> {"One", "Two", "Three", "Four"}, new List<bool> { false, false, true, false}}
        };

        [Theory]
        [MemberData(nameof(ValidData))]
        public void CreateQuestion_WhenValidAnswersProvided_ShouldNotHaveError(List<string> name, List<bool> isRight)
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            for (var i = 0; i < name.Count; i++)
            {
                model.Answers.Add(new AnswerDto { NameAnswer = name[i], IsRight = isRight[i] });
            }

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldNotHaveValidationErrorFor(x => x.Answers);
        }
    }
}
