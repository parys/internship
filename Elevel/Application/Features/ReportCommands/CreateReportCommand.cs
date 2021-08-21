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
                
                RuleFor(x => x).NotEmpty();

                When(x => x.QuestionId.HasValue && !x.AuditionId.HasValue, () =>
                {
                    RuleFor(x => x.QuestionId).NotEmpty().NotEqual(Guid.Empty)
                        .WithMessage("When QuestionId is given, AuditionId and TopicId must be empty.");
                    RuleFor(x => x.TopicId).Null()
                        .WithMessage("When QuestionId is given, AuditionId and TopicId must be empty.");
                });

                When(x => x.AuditionId.HasValue, () =>
                {
                    RuleFor(x => x.AuditionId).NotEmpty().NotEqual(Guid.Empty)
                        .WithMessage("When QuestionId and AuditionId is given, TopicId must be empty.");
                    RuleFor(x => x.QuestionId).NotEmpty().NotEqual(Guid.Empty)
                        .WithMessage("When QuestionId and AuditionId is given, TopicId must be empty.");
                    RuleFor(x => x.TopicId).Null()
                        .WithMessage("When QuestionId and AuditionId is given, TopicId must be empty.");
                });

                When(x => x.TopicId.HasValue, () =>
                {
                    RuleFor(x => x.QuestionId).Null()
                        .WithMessage("When TopicId is given, AuditionId and QuestionId must be empty.");
                    RuleFor(x => x.AuditionId).Null()
                        .WithMessage("When TopicId is given, AuditionId and QuestionId must be empty.");
                    RuleFor(x => x.TopicId).NotEmpty().NotEqual(Guid.Empty)
                        .WithMessage("When TopicId is given, AuditionId and QuestionId must be empty.");
                });

                RuleFor(x => x).Must(x =>
                    x.QuestionId.HasValue && x.QuestionId != Guid.Empty ||
                    x.AuditionId.HasValue && x.AuditionId != Guid.Empty ||
                    x.TopicId.HasValue && x.TopicId != Guid.Empty)
                    .WithMessage("At least one field QuestionId, AuditionId or TopicId must have a value");
            }
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
                Guid creatorId = Guid.Empty;

                var report = _mapper.Map<Report>(request);
                
                if (request.AuditionId == null && request.QuestionId != null)
                {
                    var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId, cancellationToken: cancellationToken);
                    
                    if (question == null)
                    {
                        throw new NotFoundException(nameof(question), request.QuestionId);
                    }

                    creatorId = question.CreatorId;
                }
                else if(request.AuditionId != null && request.QuestionId != null)
                {
                    var audition = await _context.Auditions.FirstOrDefaultAsync(x => x.Id == request.AuditionId, cancellationToken: cancellationToken);

                    if (audition == null)
                    {
                        throw new NotFoundException(nameof(audition), request.AuditionId);
                    }

                    creatorId = audition.CreatorId;
                }
                else if(request.TopicId != null)
                {
                    var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId, cancellationToken: cancellationToken);

                    if (topic == null)
                    {
                        throw new NotFoundException(nameof(topic), request.TopicId);
                    }

                    creatorId = topic.CreatorId;
                }

                report.CreatorId = creatorId;

                _context.Reports.Add(report);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response {Id = report.Id};
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}