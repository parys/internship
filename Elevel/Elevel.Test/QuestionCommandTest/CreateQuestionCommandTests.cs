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

        public static IEnumerable<object[]> ValidData => new List<object[]>
        {
            new object[] {new List<string> {"Nitwit", "Blubber", "Oddment", "Tweat"}, new List<bool> { false, true, false, false} },
            new object[] {new List<string> {"Fall", "Winter", "Spring", "Summer"}, new List<bool> { false, false, false, true}},
            new object[] {new List<string> {"One", "Two", "Three", "Four"}, new List<bool> { false, false, true, false}}
        };

        public static IEnumerable<object[]> UnvalidData => new List<object[]>
        {
            new object[] {new List<string> {"That", "is", "enough"}, new List<bool> { false, true, false}},
            new object[] {new List<string> {"Every", "Answer", "Is", "Fine"}, new List<bool> { true, true, true, true } },
            new object[] {new List<string> {"There", "Is", "no", "answer"}, new List<bool> {false, false, false, false } },
            new object[] {new List<string> {"Two", "errors", "at once"}, new List<bool> { false, true, true}}
        };

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

        [Theory]
        [MemberData(nameof(ValidData))]
        public void CreateQuestion_WhenValidAnswersProvided_ReturnsOk(List<string> name, List<bool> isRight)
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            for (var i = 0; i < name.Count; i++)
            {
                model.Answers.Add(new AnswerDto { NameAnswer = name[i], IsRight = isRight[i] });
            }

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldNotHaveValidationErrorFor(x => x.Answers);
        }

        [Theory]
        [MemberData(nameof(UnvalidData))]
        public void CreateQuestion_WhenNotFourAnswersProvided_ReturnsValidationError(List<string> name, List<bool> isRight)
        {
            var model = new Request
            {
                Answers = new List<AnswerDto>()
            };

            for (var i = 0; i < name.Count; i++)
            {
                model.Answers.Add(new AnswerDto { NameAnswer = name[i], IsRight = isRight[i] });
            }

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Answers));
            result.ShouldHaveValidationErrorFor(x => x.Answers);
        }
    }
}
