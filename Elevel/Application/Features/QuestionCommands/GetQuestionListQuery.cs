using AutoMapper;
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

                return new Response
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    Results = await questions.Skip(request.SkipCount())
                    .Take(request.PageSize)
                    .ProjectTo<QuestionsDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                    RowCount = await questions.CountAsync(cancellationToken)
                };
            }
        }

        [Serializable]
        public class Response : PagedResult<QuestionsDTO>
        {

        }

        [Serializable]
        public class QuestionsDTO
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public bool Deleted { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid AnswerId { get; set; }
            public Guid? AuditionId { get; set; }
        }
    }
}
