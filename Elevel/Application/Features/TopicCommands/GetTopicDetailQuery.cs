using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class GetTopicDetailQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid? Id { get; set; }
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

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var topic = await _context.Topics.AsNoTracking()
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.TopicName == request.TopicName && a.CreationDate == request.CreationDate, cancellationToken);

                if (topic == null)
                {
                    return null;
                }

                topic = _mapper.Map(request, topic);

                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = topic.Id };
            }
        }


        public class Response
        {
            public Guid ? Id { get; set; }
            public string TopicName { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
