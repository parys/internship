using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class DeleteTopicCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public DeleteTopicCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteTopicCommand command, CancellationToken cancellationToken)
            {
                var topic = await _context.Topics.Where(x => x.Id == command.Id).FirstOrDefaultAsync();
                if (topic == null)
                    return default;
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
                return topic.Id;
            }
        }
    }
}