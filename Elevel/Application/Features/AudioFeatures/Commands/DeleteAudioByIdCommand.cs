using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.AudioFeatures.Commands
{
    public class DeleteAudioByIdCommand: IRequest<Guid>
    {
        public Guid Id { get; set; }
        public class DeleteAudioByIdCommandHandler: IRequestHandler<DeleteAudioByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;
            public DeleteAudioByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Guid> Handle(DeleteAudioByIdCommand command, CancellationToken cancellationToken)
            {
                var audio = await _context.Auditions.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                if (audio == null) return default;
                _context.Auditions.Remove(audio);
                await _context.SaveChanges();
                return audio.Id;
            } 
        }
    }
}
