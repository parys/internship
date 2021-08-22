using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
                    .AsNoTracking()
                    .Where(x=>x.ReportStatus == ReportStatus.Created);

                return new Response()
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    RowCount = await dbReport.CountAsync(cancellationToken),
                    Results = await dbReport.Skip(request.SkipCount())
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