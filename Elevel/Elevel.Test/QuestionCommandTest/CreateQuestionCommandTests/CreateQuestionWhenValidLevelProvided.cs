using Elevel.Domain.Enums;
using FluentValidation.TestHelper;
using System;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenValidLevelProvided
    {
        private readonly Validator _validator;
        public CreateQuestionWhenValidLevelProvided()
        {
            _validator = new Validator();
        }

        [Fact]
        public void CreateQuestion_WhenLevelProvided_Execute()
        {
            Random rnm = new();
            var model = new Request
            {
                Level = (Level)(rnm.Next(1, 5))
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldNotHaveValidationErrorFor(x => x.Level);
        }
    }
}
