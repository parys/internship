using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            public long QuestionNumber { get; set; }
            public List<Answer> Answers { get; set; }
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
                    return null;
                }
                question.NameQuestion = request.NameQuestion;
                question.Level = request.Level;
                question.QuestionNumber = request.QuestionNumber;
                question.Answers = request.Answers;
                question = _mapper.Map(request, question);

                await _context.SaveChangesAsync(cancelationtoken);
                return new Response { Id = question.Id };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}
