using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class UpdateTopicCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string TopicName { get; set; }
            public Level Level { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.TopicName).NotEmpty().WithMessage("The topic title can't be empty or null!");
                RuleFor(x => x.Level).IsInEnum().WithMessage("The level must be between 1 and 5!");
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
                var validator = new Validator();
                validator.Validate(request, options =>
                {
                    options.ThrowOnFailures();
                    options.IncludeProperties(x => x.TopicName);
                    options.IncludeProperties(x => x.Level);
                });

                var topic = await _context.Topics
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (topic == null)
                {
                    throw new NotFoundException($"The topic with the ID = {request.Id}");
                }

                topic = _mapper.Map(request, topic);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = topic.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }

    }
}
