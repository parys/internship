using Xunit;
using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using AnswerDto = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.AnswerDto;
using FluentValidation.TestHelper;
using System.Collections.Generic;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenInvalidAnswersProvided
    {
        private readonly Validator _validator;

        public CreateQuestionWhenInvalidAnswersProvided()
        {
            _validator = new Validator();
        }

        public static IEnumerable<object[]> UnvalidData => new List<object[]>
        {
            new object[] {new List<string> {"That", "is", "enough"}, new List<bool> { false, true, false}},
            new object[] {new List<string> {"Every", "Answer", "Is", "Fine"}, new List<bool> { true, true, true, true } },
            new object[] {new List<string> {"There", "Is", "no", "answer"}, new List<bool> {false, false, false, false } },
            new object[] {new List<string> {"Two", "errors", "at once"}, new List<bool> { false, true, true}}
        };

        [Theory]
        [MemberData(nameof(UnvalidData))]
        public void CreateQuestion_WhenInvalidAnswersProvided_Execute(List<string> name, List<bool> isRight)
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
            result.ShouldHaveValidationErrorFor(x => x.Answers);
        }
    }
}
