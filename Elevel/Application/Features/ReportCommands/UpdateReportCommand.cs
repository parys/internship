using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace Elevel.Application.Features.ReportCommands
{
    public class UpdateReportCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            [JsonIgnore] 
            public Guid CoachId { get; set; }
            public ReportStatus ReportStatus { get; set; }
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
                var report = await _context.Reports
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (report is null)
                {
                    throw new NotFoundException(nameof(Report), request.Id);
                }
                
                report = _mapper.Map(request, report);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = report.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}