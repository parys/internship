using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TopicCommands
{
    public class CreateTopicCommand
    {
        public class Request : IRequest<Response>
        {
            public string TopicName {get; set;}
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
                var topic = _mapper.Map<Topic>(request);

                _context.Topics.Add(topic);
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
