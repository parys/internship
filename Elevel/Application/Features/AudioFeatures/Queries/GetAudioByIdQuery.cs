using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Elevel.Domain.Models;
using Elevel.Application.Interfaces;
using System.Linq;
using System;

namespace Elevel.Application.Features.AudioFeatures.Queries
{
    public class GetAudioByIdQuery: IRequest<Audition>
    {
        public Guid Id { get; set; }
        public class GetAudioByIdQueryHandler: IRequestHandler<GetAudioByIdQuery, Audition>
        {
            private IApplicationDbContext _context;
            public GetAudioByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Audition> Handle(GetAudioByIdQuery query, CancellationToken cancellationToken)
            {
                var audio = _context.Auditions.Where(a => a.Id == query.Id).FirstOrDefault();
                if (audio == null) return null;
                return audio;
            }
        }
    }
}
