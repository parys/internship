using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class GetTopicByIdQuery : IRequest<Topic>
    {
       public Guid Id { get; set; }
        public class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Topic>
        {
            private readonly IApplicationDbContext _context;
            public GetTopicByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Topic> Handle(GetTopicByIdQuery query, CancellationToken cancellationToken)
            {
                var topic = _context.Topics.Where(x => x.Id == query.Id).FirstOrDefault();
                if(topic == null)
                {
                    return null;
                }
                return topic;
            }
        }
    }
}
