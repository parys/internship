using Elevel.Domain.Enums;
using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.UpdateQuestionCommandTests
{
    public class UpdateQuestionWhenInvalidLevelProvided
    {
        private readonly Validator _validator;
        public UpdateQuestionWhenInvalidLevelProvided()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData((Level)0)]
        [InlineData((Level)42)]
        public void UpdateQuestion_WhenInvalidLevelProvided_Execute(Level level)
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
