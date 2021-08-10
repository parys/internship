using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class CreateQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid CreatorId { get; set; }

            public string NameQuestion { get; set; }

            public Level Level { get; set; }

            public List<AnswerDto> Answers { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.NameQuestion)
                    .NotEmpty()
                    .WithMessage("The question name can't be empty or null!");

                RuleFor(x => x.Level)
                    .IsInEnum()
                    .WithMessage("The level must be between 1 and 5!");

                RuleFor(x => x.Answers)
                    .Must(x => x.Count == Constants.ANSWER_AMOUNT)
                    .WithMessage($"The amount of answers must be {Constants.ANSWER_AMOUNT}");

                RuleFor(x => x.Answers)
                    .Must(x => x.Where(a => a.IsRight).Count() == Constants.CORRECT_ANSWER_AMOUNT)
                    .WithMessage($"Only {Constants.CORRECT_ANSWER_AMOUNT} answer can be right");
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {

                var question = _mapper.Map<Question>(request);
                question.Id = Guid.NewGuid();

                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync(cancelationtoken);

                return _mapper.Map<Response>(question);
            }
        }
        public class Response
        {
            public Guid Id { get; set; }

            public long QuestionNumber { get; set; }

            public string NameQuestion { get; set; }

            public DateTimeOffset CreationDate { get; set; }
        }

        public class AnswerDto
        {
            public string NameAnswer { get; set; }

            public bool IsRight { get; set; }
        }
    }
}
