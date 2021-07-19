using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class CreateTopicCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public bool Deleted { get; set; }

        public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CreateTopicCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
            {
                var topic = new Topic();
                topic.TopicName = command.TopicName;
                topic.Level = command.Level;
                topic.CreationDate = command.CreationDate;
                topic.Deleted = command.Deleted;
                _context.Topics.Add(topic);
                await _context.SaveChangesAsync();
                return topic.Id;
            }

            
        }
    }
}
