﻿using System;
using System.Threading.Tasks;
using MediatR;
using Elevel.Application.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;


namespace Elevel.Application.Features.AudioFeatures
{
   public class DeleteAudiotionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id{ get; set; }
            public string AudioFilePath { get; set; }
            public DateTimeOffset CreationDate { get; set; }
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
                var audiotion = await _context.Auditions.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (audiotion is null)
                {
                    throw new NullReferenceException();
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
