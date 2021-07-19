using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class GetAllTopicsQuery : IRequest<IEnumerable<Topic>>
    {
        public class GetAllTopicsQueryHandler : IRequestHandler<GetAllTopicsQuery, IEnumerable<Topic>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllTopicsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
             public async Task<IEnumerable<Topic>> Handle(GetAllTopicsQuery query, CancellationToken cancellationToken)
            {
                var topicList = await _context.Topics.ToListAsync();
                if(topicList == null)
                {
                    return null;
                }
                return topicList.AsReadOnly();
            }
        }
    }
}
