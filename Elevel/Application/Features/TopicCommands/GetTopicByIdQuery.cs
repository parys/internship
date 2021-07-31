using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class GetTopicByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
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
                var topic = await _context.Topics.AsNoTracking()
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (topic == null)
                {
                    throw new NotFoundException($"The topic with the ID = {request.Id}");
                }

                return topic;
            }
        }

        [Serializable]
        public class Response
        {
            public Guid Id { get; set; }
            public string TopicName { get; set; }
            public Level Level { get; set; }
            public Guid CreatorId { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
