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
        public class Request : UpsertTopicCommand.Request, IRequest<Response>
        {

        }

        public class Validator : UpsertTopicCommand.Validator<Request>
        {
            public Validator()
            {

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
                var entity = _mapper.Map<Topic>(request);
                _context.Topics.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = entity.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}
