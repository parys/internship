using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class UpdateQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }

            public string NameQuestion { get; set; }

            public Level Level { get; set; }

            public List<AnswerDto> Answers { get; set; }
        }

        public class Validator: AbstractValidator<Request>
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
                    .Must(x => x.Count(x => x.IsRight) == Constants.CORRECT_ANSWER_AMOUNT)
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
                var question = await _context.Questions.Include(x => x.Answers).FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);

                if (question is null)
                {
                    throw new NotFoundException($"Question with id {request.Id}");
                }

                var answers = question.Answers.ToList();

                if (!request.Answers.All(x => answers.Any(y => y.Id == x.Id)))
                {
                    throw new ValidationException($"This question doesn't contain answers you sent");
                }

                foreach (var answer in answers)
                {
                    var requestAnswer = request.Answers.FirstOrDefault(x => x.Id == answer.Id);

                    answer.NameAnswer = requestAnswer.NameAnswer;
                    answer.IsRight = (bool)requestAnswer.IsRight;
                }

                question.Level = request.Level;
                question.NameQuestion = request.NameQuestion;

                await _context.SaveChangesAsync();
                return new Response { Id = question.Id };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
        }

        public class AnswerDto
        {
            public Guid? Id { get; set; }
            public string NameAnswer { get; set; }
            public bool IsRight { get; set; }
        }
    }
}
