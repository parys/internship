using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Elevel.Application.Pagination;
using AutoMapper.QueryableExtensions;

namespace Elevel.Application.Features.AuditionFeatures
{
    public class GetAuditionListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
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
                var audition = _context.Auditions.AsNoTracking();

                return new Response
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    Results = await audition.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<AuditionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                    RowCount = await audition.CountAsync(cancellationToken)
                };
            }
        }
        [Serializable]
        public class Response : PagedResult<AuditionDto>
        {

        }
        [Serializable]
        public class AuditionDto
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
