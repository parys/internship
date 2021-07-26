using System;
using System.Threading.Tasks;
using MediatR;
using Elevel.Application.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Elevel.Application.Infrastructure;
using Elevel.Domain.Models;

namespace Elevel.Application.Features.AuditionFeatures
{
    public class UpdateAuditionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
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
                var audition = await _context.Auditions.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (audition is null)
                {
                    return null;
                }

                audition = _mapper.Map(request, audition);

                await _context.SaveChangesAsync(cancelationtoken);
                return new Response { Id = audition.Id, AuditionFilePath = audition.AudioFilePath };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
            public string AuditionFilePath { get; set; }
        }
    }
}
