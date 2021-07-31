using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
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
                if (request.Id.HasValue)
                {
                    tests = tests.Where(x => x.Id == request.Id);
                }

                var testsList = _mapper.Map<List<TestDTO>>(tests);

                return new Response()
                {
                    Tests = testsList
                };
            }
        }

        public class Response
        {
            public List<TestDTO> Tests { get; set; }
        }
        public class TestDTO
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