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
using Elevel.Domain.Models;

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
                var audition = await _context.Auditions.IgnoreQueryFilters()
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                
                if (audition is null)
                {
                    throw new NotFoundException(nameof(Audition), request.Id);
                }

                var response = _mapper.Map<Response>(audition);
                return response;
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public long AuditionNumber { get; set; }
            public string AudioFilePath { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid CreatorId { get; set; }
            public IEnumerable<QuestionDto> Questions { get; set; }
        }

        public class QuestionDto
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public Level Level { get; set; }
            public IEnumerable<AnswerDto> Answers { get; set; }
            
        }

        public class AnswerDto
        {
            public Guid Id { get; set; }
            public String NameAnswer { get; set; }
            public bool IsRight { get; set; }
        }
    }
}
