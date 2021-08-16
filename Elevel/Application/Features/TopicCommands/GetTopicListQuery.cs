using AutoMapper;
using Elevel.Application.Extensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class GetTopicListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Level? Level { get; set; }
            public string TopicName { get; set; }
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

                if (!string.IsNullOrEmpty(request.TopicName))
                {
                    topic = topic.Where(x => x.TopicName.StartsWith(request.TopicName));
                }

                Expression<Func<Topic, object>> sortBy = x => x.TopicName;
                Expression<Func<Topic, object>> thenBy = x => x.Level;
                if (!string.IsNullOrWhiteSpace(request.SortOn))
                {
                    if (request.SortOn.Contains(nameof(Topic.TopicName),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.TopicName;
                        thenBy = x => x.Level;
                    }
                    else if (request.SortOn.Contains(nameof(Topic.Level),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.Level;
                        thenBy = x => x.TopicName;
                    }
                    else if (request.SortOn.Contains(nameof(Topic.CreationDate),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.CreationDate;
                        thenBy = x => x.TopicName;
                    }
                }

                return await topic.GetPagedAsync<Response, Topic, TopicListDto>(request, _mapper, sortBy, thenBy);
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
            public Guid CreatorId { get; set; }
        }
    }
}
