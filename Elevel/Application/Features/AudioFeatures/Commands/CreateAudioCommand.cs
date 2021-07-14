using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Elevel.Application.Interfaces;

namespace Elevel.Application.Features.AudioFeatures.Commands
{
    public class CreateAudioCommand: IRequest<Guid>
    {
        public string AudioFilePath { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Test> Tests { get; set; }
        public class CreateAudioCommandHandler: IRequestHandler<CreateAudioCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public CreateAudioCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(CreateAudioCommand command, CancellationToken cancellationToken)
            {
                var audio = new Audition();
                audio.AudioFilePath = command.AudioFilePath;
                audio.Level = command.Level;
                audio.CreationDate = command.CreationDate;
                audio.Questions = command.Questions;
                audio.Tests = command.Tests;
                _context.Auditions.Add(audio);
                await _context.SaveChanges();
                return audio.Id;
            }
        }
    }
}
