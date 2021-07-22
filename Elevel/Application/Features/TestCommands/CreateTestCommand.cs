using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class CreateTestCommand 
    {
        public class Request : IRequest<Response>
        {
            public Level Level { get; set; }
            public DateTimeOffset AssignmentEndDate { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid UserId { get; set; }
            public Guid? HrId { get; set; }
            public TestPriority Priority { get; set; }
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
                var test = _mapper.Map<Test>(request);

                _context.Tests.Add(test);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = test.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}