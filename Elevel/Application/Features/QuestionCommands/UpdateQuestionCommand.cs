﻿using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class UpdateQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public bool Deleted { get; set; }
            public Guid AnswerId { get; set; }
            public Guid? AuditionId { get; set; }
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
                var question = await _context.Questions.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);
                if (question is null)
                {
                    return null;
                }
                question = _mapper.Map(request, question);

                await _context.SaveChangesAsync(cancelationtoken);
                return new Response { Id = question.Id };
            }
        }
        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}