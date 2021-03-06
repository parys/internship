using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

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
                bool isRightAnswer = true;

                var report = await _context.Reports
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (report.CreatorId != request.CoachId)
                {
                    throw new ValidationException("You can't change this report status");
                }

                if(report.ReportStatus == ReportStatus.Fixed || report.ReportStatus == ReportStatus.Declined)
                {
                    throw new ValidationException("This report has already been solved");
                }

                if (report is null)
                {
                    throw new NotFoundException(nameof(Report), request.Id);
                }

                var userEmail = (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == report.UserId)).Email;
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    throw new ArgumentException($"User email couldn't be null or empty.");
                }

                if (request.ReportStatus == ReportStatus.Fixed)
                {
                    var test = await _context.Tests.FirstOrDefaultAsync(x => x.Id == report.TestId);
                    if(test is null)
                    {
                        throw new NotFoundException("Test", test);
                    }

                    isRightAnswer = (await _context.TestQuestions
                        .Include(x => x.UserAnswer)
                        .FirstOrDefaultAsync(x => x.TestId == report.TestId
                            && (report.QuestionId.HasValue ? x.QuestionId == report.QuestionId : true)))
                        .UserAnswer.IsRight;

                    if (report.QuestionId.HasValue && report.AuditionId.HasValue && !report.TopicId.HasValue)
                    {
                        if (!isRightAnswer)
                        {
                            test.AuditionMark++;
                        }
                    }
                    else if (report.QuestionId.HasValue && !report.AuditionId.HasValue && !report.TopicId.HasValue)
                    {
                        if (!isRightAnswer)
                        {
                            test.GrammarMark++;
                        }
                    }
                }

                report = _mapper.Map(request, report);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.ReportStatus == ReportStatus.Fixed)
                {
                    _mailService.SendMessage(userEmail,
                        "Reported mistake has been resolved.",
                        $"Your reported mistake has been resolved by Coach.< br/>"
                        + $"{(isRightAnswer ? $"Thanks for reporting us!" : $"In thanks for reporting us one point is added to the score of test result.")}"
                        + "Please go to the following link to enter the Elevel site: <br/>"
                        + "<a href=\"http://exadel-train-app.herokuapp.com/\">Enter the Elevel site</a><br/><br/>"
                        + "Best regards, <br/>Elevel team");
                }
                else if (request.ReportStatus == ReportStatus.Declined)
                {
                    _mailService.SendMessage(userEmail,
                        "Mistake is investigated.",
                        $"Your reported mistake has been investigated by Coach.<br/>"
                        + "The reported mistake doesn't contain mistake. <br/>"
                        + "Thanks for reporting us!<br/>"
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