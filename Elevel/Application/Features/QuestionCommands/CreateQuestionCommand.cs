using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class CreateQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            public string NameQuestion { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid AnswerId { get; set; }
            public Guid? AuditionId { get; set; }
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

                _context.Questions.Add(question);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = question.Id };
            }
        }
        public class Response
        {
            public Guid? Id { get; set; }
        }
    }
}
