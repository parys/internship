using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
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
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var topic = await _context.Topics
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (topic == null)
                {
                    throw new NotFoundException(nameof(Topic), request.Id);
                }
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = topic.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}