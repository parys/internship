using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenValidNameProvided
    {

        private readonly Validator _validator;
        public CreateQuestionWhenValidNameProvided()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData("Valid data")]
        [InlineData("vfnfkevejkdfvg")]
        [InlineData("пример")]
        [InlineData("64815156")]
        public void CreateQuestion_WhenNameProvided_Execute(string name)
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
