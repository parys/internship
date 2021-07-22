using System;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Application.Features.AuditionFeatures
{
   public class DeleteAuditionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var audiotion = await _context.Auditions.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (audiotion is null)
                {
                    return null;
                }
                _context.Auditions.Remove(audiotion);

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
