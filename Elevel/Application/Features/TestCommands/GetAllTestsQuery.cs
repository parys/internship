using AutoMapper;
using Elevel.Application.Extensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
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

            public Guid? Id { get; set; }
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
                if (request.Id.HasValue)
                {
                    tests = tests.Where(x => x.Id == request.Id);
                }

                Expression<Func<Test, object>> sortBy = x => x.Priority;
                Expression<Func<Test, object>> thenBy = x => x.Level;
                if (!string.IsNullOrWhiteSpace(request.SortOn))
                {
                    if (request.SortOn.Contains(nameof(Test.Priority),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.Priority;
                        thenBy = x => x.Level;
                    }
                    else if (request.SortOn.Contains(nameof(Test.Level),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.Level;
                        thenBy = x => x.Priority;
                    }
                    else if (request.SortOn.Contains(nameof(Test.CreationDate),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.CreationDate;
                        thenBy = x => x.Priority;
                    }
                    else if (request.SortOn.Contains(nameof(Test.TestPassingDate),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.TestPassingDate;
                        thenBy = x => x.Priority;
                    }
                    else if (request.SortOn.Contains(nameof(Test.AssignmentEndDate),
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.AssignmentEndDate;
                        thenBy = x => x.Priority;
                    }
                }

                return await tests.GetPagedAsync<Response, Test, TestDto>(request, _mapper, sortBy, thenBy);
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

            public DateTimeOffset CreationDate { get; set; }

            public DateTimeOffset? TestPassingDate { get; set; }

            public DateTimeOffset? AssignmentEndDate { get; set; }

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