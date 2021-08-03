﻿using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class CreateQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid CreatorId { get; set; }

            public string NameQuestion { get; set; }

            public Level Level { get; set; }

            public List<AnswerDto> Answers { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            private const int ANSWER_COUNT = 4;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                if(request.Answers.Count != ANSWER_COUNT)
                {
                    throw new ValidationException("Not valid answers amount");
                }

                var question = _mapper.Map<Question>(request);
                question.Id = Guid.NewGuid();

                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync(cancelationtoken);

                return _mapper.Map<Response>(question);
            }
        }
        public class Response
        {
            public Guid Id { get; set; }

            public long QuestionNumber { get; set; }

            public string NameQuestion { get; set; }

            public DateTimeOffset CreationDate { get; set; }
        }

        public class AnswerDto
        {
            public string NameAnswer { get; set; }

            public bool IsRight { get; set; }
        }
    }
}
