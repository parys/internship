using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
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
                var report = await _context.Reports.AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (report is null)
                {
                    throw new NotFoundException(nameof(Report), request.Id);
                }

                var response = _mapper.Map<Response>(report);

                if (report.QuestionId != null)
                {
                    var question = _context.Questions.FirstOrDefaultAsync(x => x.Id == report.QuestionId, cancellationToken: cancellationToken);
                    
                    if (question is null)
                    {
                        throw new NotFoundException(nameof(User));
                    }

                    var coach = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == question.Result.CreatorId, cancellationToken);
                    
                    if (coach is null)
                    {
                        throw new NotFoundException(nameof(User));
                    }

                    response.User = new UserDTO
                    {
                        UserName = report.User.UserName
                    };

                    response.Coach = new CoachDTO
                    {
                        CoachName = coach.UserName
                    };

                }
                
                return response;
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string Description { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public ReportStatus ReportStatus { get; set; }
            public UserDTO User { get; set; }
            public CoachDTO Coach { get; set; }
        }

        public class UserDTO
        {
            public string UserName { get; set; }
        }

        public class CoachDTO
        {
            public string CoachName { get; set; }
        }
    }
}