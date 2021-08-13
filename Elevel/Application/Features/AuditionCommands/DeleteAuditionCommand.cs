using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Elevel.Application.Infrastructure;
using Elevel.Domain.Models;

namespace Elevel.Application.Features.AuditionCommands
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
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Audition audition = await _context.Auditions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                
                if (audition is null)
                {
                    throw new NotFoundException(nameof(Audition), request.Id);
                }
                
                audition.Deleted = true;
                await _context.SaveChangesAsync(cancellationToken);
                return new Response
                {
                    Id = audition.Id
                };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}
