using System;
using System.Threading.Tasks;
using MediatR;
using Elevel.Application.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;

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
                var audiotion = await _context.Auditions.FirstOrDefaultAsync(a => a.AudioFilePath == request.AudioFilePath && a.Id == request.Id, cancelationtoken);
                if (audiotion is null)
                {
                    return null;
                }

                audiotion = _mapper.Map(request, audiotion);

                await _context.SaveChangesAsync(cancelationtoken);
                return new Response { Id = audiotion.Id, AuditionFilePath = audiotion.AudioFilePath };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
            public string AuditionFilePath { get; set; }
        }
    }
}
