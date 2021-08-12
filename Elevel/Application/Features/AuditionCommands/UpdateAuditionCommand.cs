using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Elevel.Domain.Models;

namespace Elevel.Application.Features.AuditionCommands
{
    public class UpdateAuditionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
            public Level Level { get; set; }
            public List<QuestionDto> Questions { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.CascadeMode = CascadeMode.Stop;

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
                    .ForEach(x => x.SetValidator(new AuditionQuestionValidator()));
            }
        }

        public class AuditionQuestionValidator : AbstractValidator<QuestionDto>
        {
            public AuditionQuestionValidator()
            {
                this.CascadeMode = CascadeMode.Stop;

                RuleFor(x => x.NameQuestion)
                    .NotEmpty()
                    .WithMessage("The question name can't be empty or null.");

                RuleFor(x => x.Answers)
                    .NotEmpty()
                    .WithMessage("The answers couldn't be null or empty.")
                    .Must(x => x.Count == Constants.ANSWER_AMOUNT)
                    .WithMessage($"The amount of answers must be {Constants.ANSWER_AMOUNT}")
                    .Must(x => x.Count(a => a.IsRight) == Constants.CORRECT_ANSWER_AMOUNT)
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
                if (request.AudioFilePath != null && !File.Exists(request.AudioFilePath))
                {
                    throw new NotFoundException($"File with path {request.AudioFilePath} not found.");
                }

                var audition = await _context.Auditions
                    .Include(x => x.Questions)
                    .ThenInclude(x=>x.Answers)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                
                if (audition is null)
                {
                    throw new NotFoundException(nameof(Audition), request.Id);
                }

                foreach (var question in request.Questions)
                {
                    var dbQuestion = audition.Questions.FirstOrDefault(x => x.Id == question.Id);
                    
                    if (dbQuestion == null)
                    {
                        throw new ArgumentException("Invalid question Id.", nameof(request));
                    }

                    var questionsAnswersIdList = question.Answers.Select(x => x.Id);
                    var dbQuestionsAnswersIdList = dbQuestion.Answers.Select(x => x.Id);

                    if (questionsAnswersIdList.Count() != dbQuestionsAnswersIdList.Count() || 
                        !questionsAnswersIdList.All(x=> dbQuestionsAnswersIdList.Contains(x)))
                    {
                        throw new ArgumentException("Invalid answer Id.", nameof(request));
                    }

                    question.QuestionNumber = dbQuestion.QuestionNumber;
                    question.Level = request.Level;
                }

                audition = _mapper.Map(request, audition);
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
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public List<AnswerDto> Answers { get; set; }
            [JsonIgnore]
            public Level Level { get; set; }
            [JsonIgnore]
            public long QuestionNumber { get; set; }
        }

        public class AnswerDto
        {
            public Guid Id { get; set; }
            public string NameAnswer { get; set; }
            public bool IsRight { get; set; }
        }
    }
}
