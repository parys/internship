using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;

namespace Elevel.Domain.Validators
{
    public class TopicValidator : AbstractValidator<Topic>
    {
        public TopicValidator(IHttpContextAccessor _httpContextAccessor)
        {
            RuleFor(topic => topic.TopicName)
                .Must(topicName => NameRule(topicName, _httpContextAccessor.HttpContext.Request.Method))
                .WithMessage("The name of the topic can't be empty or null!");
            RuleFor(topic => topic.Level)
                .Must(Level => LevelRule(Level, _httpContextAccessor.HttpContext.Request.Method))
                .WithMessage("The value of level isn't defined for enum Level (the allowed are from 1 to 5)");
        }

        private bool NameRule(string name, string method)
        {
            if (method.ToUpper() == "PUT" || method.ToUpper() == "POST")
            {
                return !string.IsNullOrEmpty(name);
            }
            else
            {
                return true;
            }
        }

        private bool LevelRule(Level level, string method)
        {
            if (method.ToUpper() == "PUT" || method.ToUpper() == "POST")
            {
                return Enum.IsDefined(typeof(Level), level);
            }
            else
            {
                return true;
            }
        }
    }
}
