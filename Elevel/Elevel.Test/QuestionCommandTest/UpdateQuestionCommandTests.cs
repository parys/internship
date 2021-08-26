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

        [Fact]
        public void CheckIfTheTopicNameIsValid()
        {
            var model = new Request
            {
                NameQuestion = "Valid Question Name"
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldNotHaveValidationErrorFor(x => x.NameQuestion);
        }

        [Fact]
        public void CheckIfTheTopicNameIsNull()
        {
            var model = new Request
            {
                NameQuestion = null
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldHaveValidationErrorFor(x => x.NameQuestion);
        }

        [Fact]
        public void CheckIfTheTopicNameIsEmpty()
        {
            var model = new Request
            {
                NameQuestion = ""
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.NameQuestion));
            result.ShouldHaveValidationErrorFor(x => x.NameQuestion);
        }

        [Fact]
        public void CheckIfLevelIsInEnum()
        {
            Random rnm = new();
            var model = new Request
            {
                Level = (Level)(rnm.Next(1, 5))
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldNotHaveValidationErrorFor(x => x.Level);
        }

        [Fact]
        public void CheckIfLevelIsNotInEnum()
        {
            var model = new Request
            {
                Level = (Level)6
            };

            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldHaveValidationErrorFor(x => x.Level);
        }

        [Fact]
        public void CheckIfTheAnswersAreValid()
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
        public void CheckIfTheAmountOfAnswersDoesNotEqualsFour()
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
        public void CheckIfTheAmountOfRightAnswersDoesNotEqualsOne()
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
