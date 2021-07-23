using System;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
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
                var audition = await _context.Auditions.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (audition is null)
                {
                    throw new NotFoundException(nameof(Audition));
                }
                audition.Deleted = true;
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response
                {
                    Id = audition.Id,
                    Deleted = audition.Deleted
                };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
            public bool Deleted { get; set; }
        }
    }
}
