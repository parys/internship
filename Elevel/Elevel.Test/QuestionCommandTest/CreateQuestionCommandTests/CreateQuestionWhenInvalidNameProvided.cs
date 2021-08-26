using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenInvalidNameProvided
    {
        private readonly Validator _validator;
        public CreateQuestionWhenInvalidNameProvided()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        public void CreateQuestion_WhenInvalidNameProvided_Execute(string name)
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
