using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

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

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.ReportStatus).Must(x => x == ReportStatus.Declined || x == ReportStatus.Fixed)
                    .WithMessage("Invalid reportStatus");
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;
            private readonly IMailService _mailService;

            public Handler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager, IMailService mailService)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
                _mailService = mailService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var report = await _context.Reports
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (report.CreatorId != request.CoachId)
                {
                    throw new ValidationException("You can't change this report status");
                }

                if (report is null)
                {
                    throw new NotFoundException(nameof(Report), request.Id);
                }


                var userEmail = (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == report.UserId)).Email;
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    throw new NotFoundException($"User with {report.UserId}");
                }

                if (request.ReportStatus == ReportStatus.Fixed)
                {
                    var test = await _context.Tests.FirstOrDefaultAsync(x => x.Id == report.TestId);
                    if(test is null)
                    {
                        throw new NotFoundException("Test", test);
                    }

                    var IsRightAnswer = (await _context.TestQuestions
                        .Include(x => x.UserAnswer)
                        .FirstOrDefaultAsync(x => x.TestId == report.TestId
                            && (report.QuestionId.HasValue ? x.QuestionId == report.QuestionId : true)))
                        .UserAnswer.IsRight;

                    if (report.QuestionId.HasValue && report.AuditionId.HasValue && !report.TopicId.HasValue)
                    {
                        if (IsRightAnswer)
                        {
                            test.AuditionMark++;
                        }
                    }
                    else if (report.QuestionId.HasValue && !report.AuditionId.HasValue && !report.TopicId.HasValue)
                    {
                        if (IsRightAnswer)
                        {
                            test.GrammarMark++;
                        }
                    }
                }

                report = _mapper.Map(request, report);
                await _context.SaveChangesAsync(cancellationToken);

                //Add check the user answered the question correctly
                //type of bool
                var answerUser = true;

                if (request.ReportStatus == ReportStatus.Fixed)
                {
                    _mailService.SendMessage(userEmail,
                        "You get notification about received mistake report.",
                        $"The Elevel team reviewed your report and fixed the error.<br/>"
                        + "We are grateful for your help.<br/><br/>"
                        + $"{(answerUser ? $"You will be credited with 1 point." : $"")}"
                        + "Please go to the following link to enter the Elevel site: <br/>"
                        + "<a href=\"http://exadel-train-app.herokuapp.com/\">Enter the Elevel site</a><br/><br/>"
                        + "Best regards, <br/>Elevel team");
                }
                else if (request.ReportStatus == ReportStatus.Declined)
                {
                    _mailService.SendMessage(userEmail,
                        "You get notification about received mistake report.",
                        $"The Elevel team reviewed your report and declined it.<br/><br/>"
                        + "Please go to the following link to enter the Elevel site: <br/>"
                        + "<a href=\"http://exadel-train-app.herokuapp.com/\">Enter the Elevel site</a><br/><br/>"
                        + "Best regards, <br/>Elevel team");
                }

                return new Response {Id = report.Id};
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}