using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.AuditionFeatures
{
    public class GetAuditionByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var audition = await _context.Auditions.AsNoTracking()
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
                if (audition is null)
                {
                    return null;
                }
                return audition;
            }
        }
        [Serializable]
        public class Response
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
