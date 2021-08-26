using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Features.AuditionCommands;
using Elevel.Application.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.ReportCommands
{
    public class CreateReportCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid UserId { get; set; }
            public Guid? QuestionId { get; set; }
            public Guid? AuditionId { get; set; }
            public Guid? TopicId { get; set; }
            public Guid TestId { get; set; }
            public string Description { get; set; }
        }

        public class Validator : AbstractValidator<CreateReportCommand.Request>
        {
            public Validator()
            {
                this.CascadeMode = CascadeMode.Stop;
                
                When(x => (x.QuestionId.HasValue && !x.QuestionId.Equals(Guid.Empty)) && !x.AuditionId.HasValue, () =>
                {
                    RuleFor(x => x.TopicId).Null()
                        .WithMessage("When QuestionId is given, AuditionId and TopicId must be empty.");
                });

                When(x => x.AuditionId.HasValue && !x.AuditionId.Equals(Guid.Empty), () =>
                {
                    RuleFor(x => x.QuestionId).NotEmpty().NotEqual(Guid.Empty)
                        .WithMessage("When AuditionId is given, QuestionId and TopicId must be empty.");
                    RuleFor(x => x.TopicId).Null()
                        .WithMessage("When QuestionId and AuditionId is given, TopicId must be empty.");
                });

                When(x => x.TopicId.HasValue && !x.TopicId.Equals(Guid.Empty), () =>
                {
                    RuleFor(x => x.QuestionId).Null()
                        .WithMessage("When TopicId is given, AuditionId and QuestionId must be empty.");
                    RuleFor(x => x.AuditionId).Null()
                        .WithMessage("When TopicId is given, AuditionId and QuestionId must be empty.");
                });

                RuleFor(x => x).Must(x =>
                    x.QuestionId.HasValue && x.QuestionId != Guid.Empty ||
                    x.AuditionId.HasValue && x.AuditionId != Guid.Empty ||
                    x.TopicId.HasValue && x.TopicId != Guid.Empty)
                    .WithMessage("At least one field QuestionId, AuditionId or TopicId must have a value.");
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
                var report = _mapper.Map<Report>(request);
                String userEmail = "";

                if (request.AuditionId == null && request.QuestionId != null)
                {
                    var question = await _context.Questions
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == request.QuestionId, cancellationToken: cancellationToken);
                    
                    if (question == null)
                    {
                        throw new NotFoundException(nameof(question), request.QuestionId);
                    }

                    report.CreatorId = question.CreatorId;
                    
                    userEmail = (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == question.CreatorId)).Email;
                    if (string.IsNullOrWhiteSpace(userEmail))
                    {
                        throw new ArgumentException($"User email couldn't be null or empty.");
                    }
                }
                else if(request.AuditionId != null && request.QuestionId != null)
                {
                    var audition = await _context.Auditions
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == request.AuditionId, cancellationToken: cancellationToken);

                    if (audition == null)
                    {
                        throw new NotFoundException(nameof(audition), request.AuditionId);
                    }

                    report.CreatorId = audition.CreatorId;

                    userEmail = (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == audition.CreatorId)).Email;
                    if (string.IsNullOrWhiteSpace(userEmail))
                    {
                        throw new ArgumentException($"User email couldn't be null or empty.");
                    }
                }
                else if(request.TopicId != null)
                {
                    var topic = await _context.Topics
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == request.TopicId, cancellationToken: cancellationToken);

                    if (topic == null)
                    {
                        throw new NotFoundException(nameof(topic), request.TopicId);
                    }

                    report.CreatorId = topic.CreatorId;

                    userEmail = (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == topic.CreatorId)).Email;
                    if (string.IsNullOrWhiteSpace(userEmail))
                    {
                        throw new ArgumentException($"User email couldn't be null or empty.");
                    }
                }

                _context.Reports.Add(report);
                await _context.SaveChangesAsync(cancellationToken);

                _mailService.SendMessage(userEmail,
                    "You received mistake report.",
                    $"The mistake has been reported in a test. You can view more details via link: <br/>"
                    + "<a href=\"http://exadel-train-app.herokuapp.com/report\">View report</a><br/>"
                    + "Please go to the following link to enter the Elevel site: <br/>"
                    + "<a href=\"http://exadel-train-app.herokuapp.com/\">Enter the Elevel site</a><br/><br/>");

                return new Response {Id = report.Id};
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}