using AutoMapper;
using Elevel.Application.Extensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.ReportCommands
{
    public class GetReportListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Guid? CreatorId { get; set; }
            public DateTimeOffset? CreationDate { get; set; }
            public ReportStatus? ReportStatus { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;

            public Handler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dbReport = _context.Reports
                    .AsNoTracking().Where(x=>x.ReportStatus == ReportStatus.Created);

                if (request.CreatorId.HasValue)
                {
                    dbReport = dbReport.Where(x => x.CreatorId == request.CreatorId);
                }
                if (request.CreationDate.HasValue)
                {
                    dbReport = dbReport.Where(x => x.CreationDate == request.CreationDate);
                }
                if (request.ReportStatus.HasValue)
                {
                    dbReport = dbReport.Where(x => x.ReportStatus == request.ReportStatus.Value);
                }


                Expression<Func<Report, object>> sortBy = x => x.ReportStatus;
                Expression<Func<Report, object>> thenBy = x => x.CreationDate;

                if (!string.IsNullOrWhiteSpace(request.SortOn)) 
                {
                   if (request.SortOn.Contains(nameof(Report.ReportStatus),
                       StringComparison.InvariantCultureIgnoreCase))
                   {
                       sortBy = x => x.ReportStatus;
                       thenBy = x => x.CreationDate;
                   }
                   else if (request.SortOn.Contains(nameof(Report.CreationDate),
                       StringComparison.InvariantCultureIgnoreCase))
                   {
                       sortBy = x => x.CreationDate;
                       thenBy = x => x.ReportStatus;
                   }
                   else if (request.SortOn.Contains(nameof(Report.Description),
                       StringComparison.InvariantCultureIgnoreCase))
                   {
                        sortBy = x => x.Description;
                        thenBy = x => x.ReportStatus;
                   }
                }

                return await dbReport.GetPagedAsync<Response, Report, ReportDto>(request, _mapper, sortBy, thenBy);
            }
        }

        public class Response : PagedResult<ReportDto>
        {

        }

        public class ReportDto
        {
            public Guid Id { get; set; }
            public Guid? QuestionId { get; set; }
            public Guid? AuditionId { get; set; }
            public Guid? TopicId { get; set; }
            public Guid TestId { get; set; }
            public Guid UserId { get; set; }
            public Guid CoachId { get; set; }
            public string CoachName { get; set; }
            public string Description { get; set; }
            public ReportStatus ReportStatus { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}