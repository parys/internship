using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class UpdateQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid Id { get; set; }

            public string NameQuestion { get; set; }

            public Level? Level { get; set; }

            public List<AnswerDto> Answers { get; set; }
        }

        public class Validator: AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.NameQuestion).NotEmpty().WithMessage("The question name can't be empty or null!");

                RuleFor(x => x.Level).IsInEnum().WithMessage("The level must be between 1 and 5!");
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
                var question = await _context.Questions.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (question is null)
                {
                    throw new NotFoundException($"Question with id {request.Id}");
                }

                question = _mapper.Map(request, question);
                await _context.SaveChangesAsync(cancelationtoken);
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
            public bool? IsRight { get; set; }
        }
    }
}
