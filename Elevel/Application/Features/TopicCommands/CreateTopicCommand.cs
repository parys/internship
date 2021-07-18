using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class CreateTopicCommand
    {
        public class Request : IRequest<Response>
        {
            public string TopicName { get; set; }
            public DateTimeOffset CreationDate { get; set; }
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
                var topic = _mapper.Map<Topic>(request);

                _context.Topics.Add(topic);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = topic.Id };
            }
        }
        public class Response
        {
            public Guid? Id { get; set; }
        }
    }
}
