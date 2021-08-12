using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AuditionCommands
{
    public class CreateAuditionCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid CreatorId { get; set; }
            public string AudioFilePath { get; set; }
            public Level Level { get; set; }
            public List<QuestionDto> Questions { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.CascadeMode = CascadeMode.Stop;

                RuleFor(x => x.CreatorId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid creator Id.");

                RuleFor(x => x.AudioFilePath)
                    .NotEmpty()
                    .WithMessage("The file path can't be empty.");

                RuleFor(x => x.Level)
                    .IsInEnum()
                    .WithMessage("The level must be between 1 and 5.");

                RuleFor(x => x.Questions)
                    .NotEmpty()
                    .WithMessage("The questions couldn't be null or empty.")
                    .Must(x => x.Count == Constants.AUDITION_QUESTION_AMOUNT)
                    .WithMessage($"The amount of answers must be {Constants.AUDITION_QUESTION_AMOUNT}")
                    .ForEach(x=>x.SetValidator(new AuditionQuestionValidator()));
            }
        }

        public class AuditionQuestionValidator : AbstractValidator<QuestionDto>
        {
            public AuditionQuestionValidator()
            {
                this.CascadeMode = CascadeMode.Stop;

                RuleFor(x => x.CreatorId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid creator Id.");

                RuleFor(x => x.NameQuestion)
                    .NotEmpty()
                    .WithMessage("The question name can't be empty or null.");

                RuleFor(x => x.Level)
                    .IsInEnum()
                    .WithMessage("The level must be between 1 and 5.");

                RuleFor(x => x.Answers)
                    .NotEmpty()
                    .WithMessage("The answers couldn't be null or empty.")
                    .Must(x => x.Count == Constants.ANSWER_AMOUNT)
                    .WithMessage($"The amount of answers must be {Constants.ANSWER_AMOUNT}")
                    .Must(x=>x.Count(a => a.IsRight) == Constants.CORRECT_ANSWER_AMOUNT)
                    .WithMessage($"Only {Constants.CORRECT_ANSWER_AMOUNT} answer can be right.")
                    .ForEach(x => x.SetValidator(new AuditionAnswerValidator()));
            }
        }

        public class AuditionAnswerValidator : AbstractValidator<AnswerDto>
        {
            public AuditionAnswerValidator()
            {
                this.CascadeMode = CascadeMode.Stop;

                RuleFor(x => x.NameAnswer)
                    .NotEmpty()
                    .WithMessage("The question name can't be empty or null.");
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

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.AudioFilePath == null || !File.Exists(request.AudioFilePath))
                {
                    throw new NotFoundException($"File for audition not found.");
                }

                var audition = _mapper.Map<Audition>(request);
                _context.Auditions.Add(audition);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Id = audition.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }

        public class QuestionDto
        {
            public string NameQuestion { get; set; }
            public List<AnswerDto> Answers { get; set; }
            [JsonIgnore]
            public Level Level { get; set; }
            public Guid CreatorId { get; set; }

        }

        public class AnswerDto
        {
            public String NameAnswer { get; set; }
            public bool IsRight { get; set; }
        }
    }
}
