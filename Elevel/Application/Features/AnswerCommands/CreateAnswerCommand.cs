using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AnswerCommands
{
    public class CreateAnswerCommand
    {
        public class Request : IRequest<Response>
        {
            public string NameAnswer { get; set; }
            public bool IsRight { get; set; }
            public Guid QuestionId { get; set; }
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
                var answer = _mapper.Map<Answer>(request);

                _context.Answers.Add(answer);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = answer.Id };
            }
        }
        public class Response
        {
            public Guid? Id { get; set; }
        }
    }
}
