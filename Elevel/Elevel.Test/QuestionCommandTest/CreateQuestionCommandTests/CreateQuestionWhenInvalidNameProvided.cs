using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenInvalidNameProvided : CreateQuestionValidator
    {
        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        public void CreateQuestion_WhenInvalidNameProvided_ShouldHaveError(string name)
        {
            var model = new Request
            {
                NameQuestion = name
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldHaveValidationErrorFor(x => x.NameQuestion);
        }
    }
}
