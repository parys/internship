using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
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
            public string TopicName {get; set;}
            public Level Level { get; set; }
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
                var topic = _mapper.Map<Topic>(request);

                if (String.IsNullOrEmpty(topic.TopicName))
                {
                    throw new ValidationException("The name of topic can't be empty or null!");
                }

                if ((int)topic.Level < 1 || (int)topic.Level > 5)
                {
                    throw new ValidationException("The level must be held within the interval [1; 5]!");
                }

                _context.Topics.Add(topic);
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
