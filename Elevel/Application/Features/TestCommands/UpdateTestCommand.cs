using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Infrastructure;

namespace Elevel.Application.Features.TestCommands
{
    public class UpdateTestCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            public Level Level { get; set; }
            public DateTimeOffset TestPassingDate { get; set; }

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

            public bool Priority { get; set; }

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
                var test = await _context.Tests.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (test is null)
                {
                    throw new NotFoundException(nameof(Test), test);
                }
                test = _mapper.Map(request, test);
                await _context.SaveChangesAsync(cancelationtoken);
                var testResponse = _mapper.Map<Response>(test);
                return testResponse;
            }

        }
        public class Response : TestDTO
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

            public bool Priority { get; set; }
        }
    }
}