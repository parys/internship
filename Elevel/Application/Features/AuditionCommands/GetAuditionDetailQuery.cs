using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AuditionCommands
{
    public class GetAuditionDetailQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
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
                var audition = await _context.Auditions.AsNoTracking()
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (audition == null)
                {
                    throw new NotFoundException($"Audition with id {request.Id}");
                }
                return audition;
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public long AuditionNumber { get; set; }
            public string AudioFilePath { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public bool Deleted { get; set; } = false;
            public Guid CreatorId { get; set; }
            public IEnumerable<QuestionDto> Questions { get; set; }
        }

        public class QuestionDto
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public string NameAnswer { get; set; }
            public Level Level { get; set; }
            public string AudioFilePath { get; set; }
        }
    }
}
