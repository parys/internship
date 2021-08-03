﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class GetQuestionListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Level? Level { get; set; }

            public long? QuestionNumber { get; set; }

            public Guid? CreatorId { get; set; }
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
                var questions = _context.Questions.AsNoTracking();

                if (request.Level.HasValue)
                {
                    questions = questions.Where(x => x.Level == request.Level);
                }

                if (request.QuestionNumber.HasValue)
                {
                    questions = questions.Where(x => x.QuestionNumber == (long)request.QuestionNumber);
                }

                if (request.CreatorId.HasValue)
                {
                    questions = questions.Where(x => x.CreatorId == request.CreatorId);
                }

                return new Response
                {
                    questions = await questions.ProjectTo<QuestionsDTO>(_mapper.ConfigurationProvider).ToListAsync()
                };
            }
        }

        public class Response 
        {
            public IEnumerable<QuestionsDTO> questions { get; set; }
        }

        public class QuestionsDTO
        {
            public Guid Id { get; set; }

            public long QuestionNumber { get; set; }

            public byte Level { get; set; }

            public Guid CreatorId { get; set; }

            public string NameQuestion { get; set; }

            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
