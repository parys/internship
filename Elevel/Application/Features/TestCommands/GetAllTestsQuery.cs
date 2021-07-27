using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Elevel.Application.Features.TestCommands
{
    public class GetAllTestsQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Level? Level { get; set; }
            public DateTimeOffset? TestPassingDate { get; set; }
            public Guid? UserId { get; set; }
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
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var tests = _context.Tests.AsNoTracking();

                if (request.Level.HasValue)
                {
                    tests = tests.Where(x => x.Level == request.Level.Value);
                }

                if (request.TestPassingDate.HasValue)
                {
                    tests = tests.Where(x => x.TestPassingDate == request.TestPassingDate);
                }
                if (request.UserId.HasValue)
                {
                    tests = tests.Where(x => x.UserId == request.UserId);
                }


                return new Response()
                {
                    CurrentPage = request.CurrentPage,
                    PageSize = request.PageSize,
                    RowCount = await tests.CountAsync(),
                    Results = await tests.Skip(request.SkipCount()).Take(request.PageSize).ProjectTo<TestDTO>(_mapper.ConfigurationProvider).OrderBy(x => x.TestNumber).ToListAsync().ConfigureAwait(false)
                };
            }
        }

        public class Response : PagedResult<TestDTO>
        {
        }
        public class TestDTO
        {
            public Guid Id { get; set; }
            public Level Level { get; set; }
            public long TestNumber { get; set; }
            public DateTimeOffset TestPassingDate { get; set; }

            public int? GrammarMark { get; set; }
            public int? AuditionMark { get; set; }
            public int? EssayMark { get; set; }
            public int? SpeakingMark { get; set; }
            public string Comment { get; set; }

            public Guid UserId { get; set; }

            public Guid? HrId { get; set; }

            public Guid? CoachId { get; set; }

            public bool Priority { get; set; }
        }
    }
}