using Xunit;
using Validator = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Validator;
using Request = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.Request;
using AnswerDto = Elevel.Application.Features.QuestionCommands.CreateQuestionCommand.AnswerDto;
using FluentValidation.TestHelper;
using Elevel.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Elevel.Test.QuestionCommandTest
{
    public class CreateQuestionCommandTests
    {
        private readonly Validator _validator;

        public CreateQuestionCommandTests()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData("Valid data")]
        [InlineData("vfnfkevejkdfvg")]
        [InlineData("пример")]
        [InlineData("64815156")]
        public void CreateQuestion_WhenNameProvided_ReturnsOk(string name)
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
        public void CreateQuestion_WhenInvalidNameProvided_ReturnsValidationError(string name)
        {
            var model = new Request
            {
                NameQuestion = name
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldHaveValidationErrorFor(x => x.NameQuestion);
        }

        [Fact]
        public void CreateQuestion_WhenValidLevelProvided_ReturnsOk()
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
        public void CreateQuestion_WhenInvalidLevelProvided_ReturnsValidationError(Level level)
        {
            var model = new Request
            {
                Level = level
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldHaveValidationErrorFor(x => x.Level);
        }

        [Fact]
        public void CreateQuestion_WhenValidAnswersProvided_ReturnsOk()
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            model.Answers.Add(new AnswerDto {NameAnswer = "First", IsRight = true });
            model.Answers.Add(new AnswerDto { NameAnswer = "Second", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Third", IsRight = false });
            model.Answers.Add(new AnswerDto { NameAnswer = "Fourth", IsRight = false });

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldNotHaveValidationErrorFor(x => x.Answers);
        }

        [Fact]
        public void CreateQuestion_WhenNotFourAnswersProvided_ReturnsValidationError()
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
        public void CreateQuestion_WhenNotOneRightAnswerProvided_ReturnsValidationError()
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
