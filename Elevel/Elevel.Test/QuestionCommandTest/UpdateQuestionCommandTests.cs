using System;
using System.Collections.Generic;
using Validator = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Validator;
using Request = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.Request;
using AnswerDto = Elevel.Application.Features.QuestionCommands.UpdateQuestionCommand.AnswerDto;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Xunit;
using Elevel.Domain.Enums;

namespace Elevel.Test.QuestionCommandTest
{
    public class UpdateQuestionCommandTests
    {
        private readonly Validator _validator;

        public UpdateQuestionCommandTests()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData("Valid data")]
        [InlineData("vfnfkevejkdfvg")]
        [InlineData("пример")]
        [InlineData("64815156")]
        public void UpdateQuestion_WhenNameProvided_ReturnsOk(string name)
        {
            var model = new Request
            {
                NameQuestion = name
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldNotHaveValidationErrorFor(x => x.NameQuestion);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        public void UpdateQuestion_WhenInvalidNameProvided_ReturnsValidationError(string name)
        {
            var model = new Request
            {
                NameQuestion = name
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldHaveValidationErrorFor(x => x.NameQuestion);
        }

        [Fact]
        public void UpdateQuestion_WhenValidLevelProvided_ReturnsOk()
        {
            Random rnm = new();
            var model = new Request
            {
                Level = (Level)(rnm.Next(1, 5))
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldNotHaveValidationErrorFor(x => x.Level);
        }

        [Theory]
        [InlineData((Level)0)]
        [InlineData((Level)42)]
        public void UpdateQuestion_WhenInvalidLevelProvided_ReturnsValidationError(Level level)
        {
            var model = new Request
            {
                Level = level
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldHaveValidationErrorFor(x => x.Level);
        }

        [Fact]
        public void UpdateQuestion_WhenValidAnswersProvided_ReturnsOk()
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            model.Answers.Add(new AnswerDto { NameAnswer = "First", IsRight = true });
            model.Answers.Add(new AnswerDto { NameAnswer = "Second", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Third", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Fourth", IsRight = false });

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldNotHaveValidationErrorFor(x => x.Answers);
        }

        [Fact]
        public void UpdateQuestion_WhenNotFourAnswersProvided_ReturnsValidationError()
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            model.Answers.Add(new AnswerDto { NameAnswer = "First", IsRight = true });
            model.Answers.Add(new AnswerDto { NameAnswer = "Second", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Third", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Fourth", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Fifth", IsRight = false });

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldHaveValidationErrorFor(x => x.Answers);
        }

        [Fact]
        public void UpdateQuestion_WhenNotOneRightAnswerProvided_ReturnsValidationError()
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            model.Answers.Add(new AnswerDto { NameAnswer = "First", IsRight = true });
            model.Answers.Add(new AnswerDto { NameAnswer = "Second", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Third", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Fourth", IsRight = true });

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldHaveValidationErrorFor(x => x.Answers);
        }
    }
}
