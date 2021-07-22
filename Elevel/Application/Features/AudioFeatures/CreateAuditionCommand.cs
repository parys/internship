using System;
using System.Threading.Tasks;
using MediatR;
using Elevel.Application.Interfaces;
using AutoMapper;
using System.Threading;
using Elevel.Domain.Models;

namespace Elevel.Application.Features.AudioFeatures
{
    public class CreateAuditionCommand
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
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var audiotion = _mapper.Map<Audition>(request);

                _context.Auditions.Add(audiotion);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = audiotion.Id, CreationDate = DateTime.UtcNow};
            }
        }
        public class Response
        {
            public Guid? Id { get; set; }
            public DateTimeOffset CreationDate { get; set; }

        }
    }
}
