using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;

namespace Elevel.Application.Features.AuditionFeatures
{
    public class CreateAuditionCommand
    {
        public class Request : IRequest<Response>
        {
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
                var audition = _mapper.Map<Audition>(request);
                if(audition is null)
                {
                    return null;
                }
                _context.Auditions.Add(audition);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = audition.Id, CreationDate = DateTime.UtcNow };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
