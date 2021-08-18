using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class GetTestsForCoachQuery
    {
        public class Request :PagedQueryBase, IRequest<Response>
        {
            [JsonIgnore]
            public Guid CoachId { get; set; }

            public bool IsChecked { get; set; } = false;

            public DateTimeOffset? TestPassingDate { get; set; }

            public Level? Level { get; set; }

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
                var tests = _context.Tests.AsNoTracking().Where(x => x.CoachId == request.CoachId);

                if (request.IsChecked)
                {
                    tests = tests.Where(x => x.EssayMark.HasValue 
                    && x.SpeakingMark.HasValue 
                    && !string.IsNullOrWhiteSpace(x.Comment));
                } else {
                    tests = tests.Where(x => !x.EssayMark.HasValue 
                    && !x.SpeakingMark.HasValue 
                    && string.IsNullOrWhiteSpace(x.Comment));
                }

                if (request.Level.HasValue)
                {
                    tests = tests.Where(x => x.Level == request.Level.Value);
                }

                if (request.Priority.HasValue)
                {
                    tests = tests.Where(x => x.Priority);
                }

                if (request.TestPassingDate.HasValue)
                {
                    tests = tests.Where(x => x.TestPassingDate == request.TestPassingDate);
                }

                return new Response()
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    RowCount = await tests.CountAsync(cancellationToken),
                    Results = await tests.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<TestDto>(_mapper.ConfigurationProvider)
                    .OrderByDescending(x => x.Priority)
                    .ToListAsync(cancellationToken)
                };
            }
        }
        public class Response: PagedResult<TestDto>
        {
        }
        public class TestDto
        {
            public Guid Id { get; set; }
            public long TestNumber { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset TestPassingDate { get; set; }
            public bool Priority { get; set; }
            public string EssayAnswer { get; set; }
            public string SpeakingAnswerReference { get; set; }
            public int? EssayMark { get; set; }
            public int? SpeakingMark { get; set; }
            public String Comment { get; set; }
        }
    }
}
