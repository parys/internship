using Elevel.Domain.Enums;
using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenInvalidLevelProvided
    {
        private readonly Validator _validator;
        public CreateQuestionWhenInvalidLevelProvided()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData((Level)0)]
        [InlineData((Level)42)]
        public void CreateQuestion_WhenInvalidLevelProvided_Execute(Level level)
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
