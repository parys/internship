using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AuditionCommands
{
    public class CreateAuditionCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid CreatorId { get; set; }
            public long AuditionNumber { get; set; }
            public string AudioFilePath { get; set; }
            public Level Level { get; set; }
            public List<QuestionDto> Questions { get; set; }
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
                var audition = _mapper.Map<Audition>(request);
                _context.Auditions.Add(audition);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = audition.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }

        public class QuestionDto
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public string NameAnswer { get; set; }
        }
    }
}
