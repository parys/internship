using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Request;

namespace Elevel.Test.QuestionCommandTest.UpdateQuestionCommandTests
{
    public class UpdateQuestionWhenValidNameProvided : UpdateQuestionValidator
    {

        [Theory]
        [InlineData("Valid data")]
        [InlineData("vfnfkevejkdfvg")]
        [InlineData("пример")]
        [InlineData("64815156")]
        public void UpdateQuestion_WhenNameProvided_ShouldNotHaveError(string name)
        {
            var model = new Request
            {
                NameQuestion = name
            };
            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldNotHaveValidationErrorFor(x => x.NameQuestion);
        }
    }
}
