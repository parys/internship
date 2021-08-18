using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Features.AuditionCommands;
using Elevel.Application.Infrastructure;
using FluentValidation;

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
                
                When(x => x.QuestionId.HasValue, () =>
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
                var report = _mapper.Map<Report>(request);
                
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