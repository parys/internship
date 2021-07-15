﻿using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.AnswerCommands
{
    public class DeleteAnswerCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid? Id { get; set; }
            public string NameAnswer { get; set; }
            public bool IsRight { get; set; }
            public Guid QuestionId { get; set; }
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
                var answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (answer is null)
                {
                    return null;
                }
                _context.Answers.Remove(answer);

                await _context.SaveChangesAsync(cancelationtoken);
                return new Response { Id = answer.Id };
            }
        }
        public class Response
        {
            public Guid? Id { get; set; }
        }
    }
}