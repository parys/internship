using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class DeleteTopicCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid? Id { get; set; }
            public string TopicName { get; set; }
            public Level Level { get; set; }
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
                var topic = await _context.Topics.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (topic is null)
                {
                    throw new NullReferenceException();
                }
                _context.Topics.Remove(topic);

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