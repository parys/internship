using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class GetQuestionDetailQuery
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
                var question = await _context.Questions.AsNoTracking()
                    .Where(x => !x.AuditionId.HasValue)
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (question == null)
                {
                    throw new NotFoundException($"Question with id {request.Id}");
                }

                return question;
            }
        }

        public class Response
        {
            public Guid Id { get; set; }

            public byte Level { get; set; }

            public long QuestionNumber { get; set; }

            public Guid CreatorId { get; set; }

            public string NameQuestion { get; set; }

            public DateTimeOffset CreationDate { get; set; }

            public IEnumerable<AnswerDto> Answers { get; set; }
        }

        public class AnswerDto
        {
            public Guid Id { get; set; }

            public Guid QuestionId { get; set; }

            public string NameAnswer { get; set; }

            public bool IsRight { get; set; }
        }
    }
}
