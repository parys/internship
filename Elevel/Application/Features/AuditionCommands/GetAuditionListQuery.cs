using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AuditionCommands
{
    public class GetAuditionListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public long? AuditionNumber { get; set; }
            public Level? Level { get; set; }
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

                if (request.Level.HasValue)
                {
                    audition = audition.Where(x => x.Level == request.Level);
                }

                if (request.AuditionNumber.HasValue)
                {
                    audition = audition.Where(x => x.AuditionNumber == (long)request.AuditionNumber);
                }

                return new Response()
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    RowCount = await audition.CountAsync(cancellationToken),
                    Results = await audition.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
                };
            }
        }

        public class Response : PagedResult<QuestionDto>
        {

        }

        public class QuestionDto
        {
            public Guid Id { get; set; }
            public long QuestionNumber { get; set; }
            public byte Level { get; set; }
            public Guid CreatorId { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
