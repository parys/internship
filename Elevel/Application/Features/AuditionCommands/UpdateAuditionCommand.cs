using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Infrastructure;
using FluentValidation;

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

        //public class Validator : AbstractValidator<Request>
        //{

        //    public Validator()
        //    {
        //        RuleFor(x => x.NameQuestion)
        //            .NotEmpty()
        //            .WithMessage("The question name can't be empty or null!");

        //        RuleFor(x => x.Level)
        //            .IsInEnum()
        //            .WithMessage("The level must be between 1 and 5!");

        //        RuleFor(x => x.Answers)
        //            .Must(x => x.Count == Constants.ANSWER_AMOUNT)
        //            .WithMessage($"The amount of answers must be {Constants.ANSWER_AMOUNT}");

        //        RuleFor(x => x.Answers)
        //            .Must(x => x.Count(x => x.IsRight) == Constants.CORRECT_ANSWER_AMOUNT)
        //            .WithMessage($"Only {Constants.CORRECT_ANSWER_AMOUNT} answer can be right");
        //    }
        //}

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
                var audition = await _context.Auditions
                    .Include(x => x.Questions).ThenInclude(i => i.Answers)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                
                if (audition is null)
                {
                    throw new NotFoundException($"Audition with id {request.Id}");
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
            public IEnumerable<AnswerDto> Answers { get; set; }
            public Level Level { get; set; }
        }

        public class AnswerDto
        {
            public Guid Id { get; set; }
            public string NameAnswer { get; set; }
            public bool? IsRight { get; set; }
        }
    }
}
