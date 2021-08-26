using FluentValidation.TestHelper;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.UpdateQuestionCommandTests
{
    public class UpdateQuestionWhenValidNameProvided
    {

        private readonly Validator _validator;
        public UpdateQuestionWhenValidNameProvided()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData("Valid data")]
        [InlineData("vfnfkevejkdfvg")]
        [InlineData("пример")]
        [InlineData("64815156")]
        public void UpdateQuestion_WhenNameProvided_Execute(string name)
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
