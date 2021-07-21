using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
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
        public class Request : UpsertTopicCommand.Request, IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Validator : UpsertTopicCommand.Validator<Request>
        {
            public Validator()
            {
                RuleFor(v => v.Id).NotEmpty();
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
                var topic = await _context.Topics
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (topic == null)
                {
                    throw new NotFoundException(nameof(Topic));
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
