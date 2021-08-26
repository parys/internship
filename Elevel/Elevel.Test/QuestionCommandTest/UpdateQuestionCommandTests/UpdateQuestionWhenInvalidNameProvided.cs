using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.UpdateQuestionCommandTests
{
    public class UpdateQuestionWhenInvalidNameProvided
    {
        private readonly Validator _validator;
        public UpdateQuestionWhenInvalidNameProvided()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        public void UpdateQuestion_WhenInvalidNameProvided_Execute(string name)
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
