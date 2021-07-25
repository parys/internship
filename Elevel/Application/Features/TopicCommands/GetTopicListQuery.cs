using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
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
    public class GetTopicListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Level? Level { get; set; }
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
                var topic = _context.Topics.AsNoTracking();

                if (request.Level.HasValue) {
                    topic = topic.Where(x => x.Level == request.Level);
                }

                return new Response
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    Results = await topic.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<TopicListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                    RowCount = await topic.CountAsync(cancellationToken)
                };
            }
        }

        [Serializable]
        public class Response : PagedResult<TopicListDto>
        {

        }

        [Serializable]
        public class TopicListDto
        {
            public Guid Id { get; set; }
            public string TopicName { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public bool Deleted { get; set; }
        }
    }
}
