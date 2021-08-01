using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Elevel.Domain.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
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

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var topic_to_change = _mapper.Map<Topic>(request);
                var validator = new TopicValidator(_httpContextAccessor);

                validator.Validate(topic_to_change, options =>
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
