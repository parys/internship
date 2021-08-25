using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator = Elevel.Application.Features.TopicCommands.UpdateTopicCommand.Validator;
using Request = Elevel.Application.Features.TopicCommands.UpdateTopicCommand.Request;
using Xunit;
using FluentValidation.TestHelper;
using Elevel.Domain.Enums;

namespace Elevel.Test.Topic.UpdateTopicCommand
{
    public class ValidatorTests
    {
        private readonly Validator _validator;

        public ValidatorTests()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        public void RuleForTopicName_WhenTopicNameIsNullOrEmpty_ShouldHaveValidationError(string value)
        {
            var model = new Request
            {
                TopicName = value
            };
            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.TopicName));
            result.ShouldHaveValidationErrorFor(x => x.TopicName);
        }

        [Fact]
        public void RuleForTopicName_WhenValueLengthIs30_ShouldNotHaveValidationError()
        {
            var value = new string('1', 30);
            var model = new Request
            {
                TopicName = value
            };
            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.TopicName));
            result.ShouldNotHaveValidationErrorFor(x => x.TopicName);
        }

        [Fact]
        public void RulleForLevel_WhenLevelIsEmpty_ShouldHaveValidationError()
        {
            var model = new Request
            {
                Level = new Level()
            };
            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldHaveValidationErrorFor(x => x.Level);
        }

        [Theory]
        [InlineData((Level)5)]
        public void RuleForLevel_WhenCountIs5_ShouldNotHaveValidationError(Level value)
        {
            var model = new Request
            {
                Level = value
            };
            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldNotHaveValidationErrorFor(nameof(model.Level));
        }

        [Theory]
        [InlineData((Level)5)]
        public void RuleForLevel_WhenCountIs6_ShouldHaveValidationError(Level value)
        {
            var model = new Request
            {
                Level = value
            };
            var result = _validator.TestValidate(model, opt => opt.IncludeProperties(x => x.Level));
            result.ShouldNotHaveValidationErrorFor(nameof(model.Level));
        }
    }
}
