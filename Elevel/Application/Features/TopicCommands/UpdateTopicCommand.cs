using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class UpdateTopicCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool Deleted { get; set; }

        public class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public UpdateTopicCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateTopicCommand command, CancellationToken cancellationToken)
            {
                var topic = _context.Topics.Where(x => x.Id == command.Id).FirstOrDefault();

                if (topic == null)
                    return default;
                else
                {
                    topic.TopicName = command.TopicName;
                    topic.Level = command.Level;
                    topic.CreationDate = command.CreationDate;
                    topic.Deleted = command.Deleted;
                    await _context.SaveChangesAsync();
                    return topic.Id;
                }
            }
        }
    }
}
