using Elevel.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class UpsertTopicCommand
    {
        public abstract class Request
        {
            public string TopicName { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public bool Deleted { get; set; }
        }

        public abstract class Validator<T> : AbstractValidator<T> where T : Request
        {
            protected Validator()
            {

            }
        }
    }
}
