using Validator = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.UpdateQuestionCommandTests
{
    public class UpdateQuestionValidator
    {
        protected readonly Validator _validator;

        public UpdateQuestionValidator()
        {
            _validator = new Validator();
        }
    }
}