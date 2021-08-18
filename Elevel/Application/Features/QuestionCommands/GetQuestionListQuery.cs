using AutoMapper;
using Elevel.Application.Extensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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

            public string QuestionNumber { get; set; }

            public Guid? CreatorId { get; set; }

            public string NameQuestion { get; set; }
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
                var questions = _context.Questions.Include(x => x.Creator).AsNoTracking().Where(x => !x.AuditionId.HasValue);

                if (request.Level.HasValue)
                {
                    questions = questions.Where(x => x.Level == request.Level);
                }

                if (!string.IsNullOrEmpty(request.QuestionNumber))
                {
                    questions = questions.Where(x => Convert.ToString(x.QuestionNumber).StartsWith(request.QuestionNumber));
                }

                if (request.CreatorId.HasValue)
                {
                    questions = questions.Where(x => x.CreatorId == request.CreatorId);
                }

                if (!string.IsNullOrEmpty(request.NameQuestion))
                {
                    questions = questions.Where(x => x.NameQuestion.StartsWith(request.NameQuestion));
                }

                Expression<Func<Question, object>> sortBy = x => x.QuestionNumber;
                Expression<Func<Question, object>> thenBy = x => x.Level;
                if (!string.IsNullOrWhiteSpace(request.SortOn))
                {
                    if (request.SortOn.Contains(nameof(Question.QuestionNumber),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.QuestionNumber;
                        thenBy = x => x.Level;
                    }
                    else if (request.SortOn.Contains(nameof(Question.Level),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.Level;
                        thenBy = x => x.QuestionNumber;
                    }
                    else if (request.SortOn.Contains(nameof(Question.CreationDate),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.CreationDate;
                        thenBy = x => x.QuestionNumber;
                    }
                }

                return await questions.GetPagedAsync<Response, Question, QuestionsDTO>(request, _mapper, sortBy, thenBy);
            }
        }

        public class Response : PagedResult<QuestionsDTO>
        {
        }

        public class QuestionsDTO
        {
            public Guid Id { get; set; }

            public long QuestionNumber { get; set; }

            public byte Level { get; set; }

            public Guid CreatorId { get; set; }

            public string CreatorFirstName { get; set; }

            public string CreatorLastName { get; set; }

            public string NameQuestion { get; set; }

            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
