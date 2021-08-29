using Elevel.Domain.Enums;
using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Request;

namespace Elevel.Test.QuestionCommandTest.UpdateQuestionCommandTests
{
    public class UpdateQuestionWhenInvalidLevelProvided : UpdateQuestionValidator
    {

        [Theory]
        [InlineData((Level)0)]
        [InlineData((Level)42)]
        public void UpdateQuestion_WhenInvalidLevelProvided_ShouldHaveError(Level level)
        {
            var model = new Request
            {
                Level = level
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldHaveValidationErrorFor(x => x.Level);
        }
    }
}
