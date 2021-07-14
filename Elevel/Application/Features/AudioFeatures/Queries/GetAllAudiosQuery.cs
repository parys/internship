using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Elevel.Domain.Models;
using Elevel.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.AudioFeatures.Queries
{
    public class GetAllAudiosQuery: IRequest<IEnumerable<Audition>>
    {
        public class GetAllAudiosQueryHandler: IRequestHandler<GetAllAudiosQuery, IEnumerable<Audition>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllAudiosQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Audition>> Handle(GetAllAudiosQuery query, CancellationToken cancellationToken)
            {
                var audioList = await _context.Auditions.ToListAsync();
                if(audioList == null)
                {
                    return null;
                }
                return audioList.AsReadOnly();
            }
        }
    }
}
