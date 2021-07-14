using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Elevel.Application.Interfaces;
using System.Linq;

namespace Elevel.Application.Features.AudioFeatures.Commands
{
    public class UpdateAudioCommand: IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string AudioFilePath { get; set; }
        public Level Level { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Test> Tests { get; set; }
        public class UpdateAudioCommandHandler: IRequestHandler<UpdateAudioCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public UpdateAudioCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(UpdateAudioCommand command, CancellationToken cancellationToken)
            {
                var audio = _context.Auditions.Where(a => a.Id == command.Id).FirstOrDefault();
                if(audio == null)
                {
                    return default;
                }
                else
                {
                    audio.AudioFilePath = command.AudioFilePath;
                    audio.CreationDate = command.CreationDate;
                    audio.Level = command.Level;
                    audio.Questions = command.Questions;
                    audio.Tests = command.Tests;
                    await _context.SaveChanges();
                    return audio.Id;
                }
            }
        }
    }
}
