using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class GetTestByIdQuery
    {
        public class Request: IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var test = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                if(test == null)
                {
                    throw new NotFoundException($"Test with ID: {request.Id}");
                }
               
                return _mapper.Map<Response>(test);
            }
        }
        public class Response
        {
            public Guid Id { get; set; }

            public long TestNumber { get; set; }

            public Level Level { get; set; }

            public Guid UserId { get; set; }

            public DateTimeOffset TestPassingDate { get; set; }

            public Guid AuditionId { get; set; }

            public Guid EssayId { get; set; }

            public Guid SpeakingId { get; set; }
        }
    }
}
