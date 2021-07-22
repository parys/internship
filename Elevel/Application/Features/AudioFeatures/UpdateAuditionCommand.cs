using System;
using System.Threading.Tasks;
using MediatR;
using Elevel.Application.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.AudioFeatures
{
    public class UpdateAuditionCommand
    {
        public class Request: IRequest<Response>
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
            public DateTimeOffset UpdatedDate { get; set; }
        }

        public class Handler: IRequestHandler<Request, Response>
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
                request.UpdatedDate = DateTime.UtcNow;
                var audiotion = await _context.Auditions.FirstOrDefaultAsync(a => a.AudioFilePath == request.AudioFilePath && a.CreationDate == request.UpdatedDate, cancelationtoken);
                if(audiotion is null)
                {
                    return null;
                }

                audiotion = _mapper.Map(request, audiotion);

                await _context.SaveChangesAsync(cancelationtoken);
                return new Response { Id = audiotion.Id };
            }
        }
        public class Response
        {
            public Guid? Id { get; set; }
        }
    }
}
