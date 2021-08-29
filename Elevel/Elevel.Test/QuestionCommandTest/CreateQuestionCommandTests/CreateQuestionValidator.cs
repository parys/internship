using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionValidator
    {
        protected readonly Validator _validator;

        public CreateQuestionValidator()
        {
            _validator = new Validator();
        }
    }
}
