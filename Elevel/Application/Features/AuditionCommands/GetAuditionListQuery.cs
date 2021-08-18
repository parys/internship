using AutoMapper;
using Elevel.Application.Extensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AuditionCommands
{
    public class GetAuditionListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public string AuditionNumber { get; set; }
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
                var audition = _context.Auditions.Include(x => x.Creator).AsNoTracking();

                if (request.Level.HasValue)
                {
                    audition = audition.Where(x => x.Level == request.Level);
                }

                if (!string.IsNullOrEmpty(request.AuditionNumber))
                {
                    audition = audition.Where(x => Convert.ToString(x.AuditionNumber).StartsWith(request.AuditionNumber));
                }

                Expression<Func<Audition, object>> sortBy = x => x.AuditionNumber;
                Expression<Func<Audition, object>> thenBy = x => x.CreationDate;
                if (!string.IsNullOrWhiteSpace(request.SortOn))
                {
                    if (request.SortOn.Contains(nameof(Audition.AuditionNumber),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.AuditionNumber;
                        thenBy = x => x.Level;
                    }
                    else if (request.SortOn.Contains(nameof(Audition.Level),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.Level;
                        thenBy = x => x.AuditionNumber;
                    }
                    else if (request.SortOn.Contains(nameof(Audition.CreationDate),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.CreationDate;
                        thenBy = x => x.AuditionNumber;
                    }
                }

                return await audition.GetPagedAsync<Response, Audition, AuditionDto>(request, _mapper, sortBy, thenBy);
            }
        }

        public class Response : PagedResult<AuditionDto>
        {

        }

        public class AuditionDto
        {
            public Guid Id { get; set; }
            public long AuditionNumber { get; set; }
            public byte Level { get; set; }
            public Guid CreatorId { get; set; }
            public string CreatorFirstName { get; set; }
            public string CreatorLastName { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
