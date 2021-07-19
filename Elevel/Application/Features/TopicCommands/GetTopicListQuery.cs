using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class GetTopicListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
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
                var topicList = _context.Topics.AsNoTracking()
                     .Include(x => x.Id)
                     .Include(x => x.TopicName)
                     .Include(x => x.CreationDate)
                     .OrderBy(x => x.TopicName);

                var topics = new List<TopicListDto>();
                foreach (var item in topicList)
                {
                    var dto = _mapper.Map<TopicListDto>(item);
                    topics.Add(dto);
                }
                return new Response
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    Results = topics,
                    RowCount = await topicList.CountAsync(cancellationToken)
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
            public string TopicName { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
