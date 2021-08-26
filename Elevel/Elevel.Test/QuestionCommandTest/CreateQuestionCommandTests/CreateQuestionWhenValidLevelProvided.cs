using Elevel.Domain.Enums;
using FluentValidation.TestHelper;
using System;
using Xunit;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;

namespace Elevel.Test.QuestionCommandTest.CreateQuestionCommandTests
{
    public class CreateQuestionWhenValidLevelProvided : CreateQuestionValidator
    {
        [Fact]
        public void CreateQuestion_WhenLevelProvided_ShouldNotHaveError()
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
