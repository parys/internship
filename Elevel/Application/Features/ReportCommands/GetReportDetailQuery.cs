using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Elevel.Domain.Enums;

namespace Elevel.Application.Features.ReportCommands
{
    public class GetReportDetailQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
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
               var report = await _context.Reports
                    .AsNoTracking()
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (report is null)
                {
                    throw new NotFoundException(nameof(Report), request.Id);
                }

                var response = _mapper.Map<Response>(report);

                return response;
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Description { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public ReportStatus ReportStatus { get; set; }
            public string UserName { get; set; }
            public Guid UserId { get; set; }
            public string CoachName { get; set; }
            public Guid CoachId { get; set; }
        }
    }
}