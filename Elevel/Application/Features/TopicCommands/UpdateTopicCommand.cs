using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class UpdateTopicCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string TopicName { get; set; }
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
                if (String.IsNullOrEmpty(request.TopicName))
                {
                    throw new ValidationException("The name of topic can't be empty or null!");
                }

                if ((int)request.Level < 1 || (int)request.Level > 5)
                {
                    throw new ValidationException("The level field must be held within the interval [1; 5]!");
                }

                var topic = await _context.Topics
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (topic == null)
                {
                    throw new NotFoundException($"The topic with the ID = {request.Id}");
                }

                topic = _mapper.Map(request, topic);
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
