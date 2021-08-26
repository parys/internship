using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class GetTestsForAdminQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Level? Level { get; set; }

            public bool IsAssigned { get; set; } = false;

            public DateTimeOffset? TestPassingDate { get; set; }

            public bool? Priority { get; set; }
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
                var tests = _context.Tests.AsNoTracking()
                    .Where(x => !x.EssayMark.HasValue
                    && !x.SpeakingMark.HasValue && x.AuditionMark.HasValue && x.GrammarMark.HasValue);

                if (request.IsAssigned)
                {
                    tests = tests.Where(x => x.CoachId.HasValue);
                } else {
                    tests = tests.Where(x => !x.CoachId.HasValue);
                }

                if (request.Level.HasValue)
                {
                    tests = tests.Where(x => x.Level == request.Level.Value);
                }

                if (request.TestPassingDate.HasValue)
                {
                    tests = tests.Where(x => x.TestPassingDate == request.TestPassingDate);
                }

                if (request.Priority.HasValue)
                {
                    tests = tests.Where(x => x.Priority == request.Priority);
                }


                return new Response()
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    RowCount = await tests.CountAsync(cancellationToken),
                    Results = await tests.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<TestDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                };
            }
        }

        public class Response : PagedResult<TestDto>
        {
        }
        public class TestDto
        {
            public Guid Id { get; set; }

            public Level Level { get; set; }

            public long TestNumber { get; set; }

            public DateTimeOffset TestPassingDate { get; set; }

            public Guid? CoachId { get; set; }

            public bool Priority { get; set; }
        }
    }
}
