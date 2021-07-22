using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

                if (request.Level!=null)
                {
                    tests = tests.Where(x => x.Level == request.Level);
                }

                if (request.TestPassingDate != null)
                {
                    tests = tests.Where(x => x.TestPassingDate == request.TestPassingDate);
                }

                return new Response()
                {
                    CurrentPage = request.CurrentPage,
                    PageSize = request.PageSize,
                    RowCount = await tests.CountAsync(),
                    Results = await tests.Skip(request.SkipCount()).Take(request.PageSize).ProjectTo<TestDTO>(_mapper.ConfigurationProvider).ToListAsync()
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
            public DateTimeOffset CreationDate { get; set; }
            public DateTimeOffset TestPassingDate { get; set; }
            public DateTimeOffset AssignmentEndDate { get; set; }

            public int? GrammarMark { get; set; }
            public int? AuditionMark { get; set; }
            public int? EssayMark { get; set; }
            public int? SpeakingMark { get; set; }

            public string EssayAnswer { get; set; }
            public string SpeakingAnswerReference { get; set; }
            public string Comment { get; set; }

            public Guid UserId { get; set; }

            public Guid? HrId { get; set; }

            public Guid? CoachId { get; set; }

            public Guid? AuditionId { get; set; }

            public Guid? EssayId { get; set; }
            public Guid? SpeakingId { get; set; }

            public TestPriority Priority { get; set; }
        }
    }
}