using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class GetQuestionListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public DateTimeOffset CreationDate { get; set; }
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

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var questionList = _context.Questions.AsNoTracking()
                    .Include(x => x.Id)
                    .Include(x => x.NameQuestion)
                    .Include(x => x.CreationDate)
                    .Include(x => x.AnswerId)
                    .Include(x => x.AuditionId)
                    .OrderBy(x => x.NameQuestion);

                var questions = new List<QuestionListDto>();
                foreach (var item in questionList)
                {
                    var dto = _mapper.Map<QuestionListDto>(item);
                    questions.Add(dto);
                }
                return new Response
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    Results = questions,
                    RowCount = await questionList.CountAsync(cancellationToken)
                };
            }
        }

        [Serializable]
        public class Response : PagedResult<QuestionListDto>
        {

        }

        [Serializable]
        public class QuestionListDto
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid AnswerId { get; set; }
            public Guid? AuditionId { get; set; }
        }
    }
}
