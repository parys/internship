using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.ReportCommands
{
    public class GetReportListQuery
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
                var report = _context.Reports.AsNoTracking().Where(x=>x.ReportStatus == ReportStatus.Created);

                return new Response()
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    RowCount = await report.CountAsync(cancellationToken),
                    Results = await report.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
                };
            }
        }

        public class Response : PagedResult<ReportDto>
        {

        }

        public class ReportDto
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public Guid CoachId { get; set; }
            public string Description { get; set; }
            public ReportStatus ReportStatus { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}