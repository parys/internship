using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.QuestionCommands
{
    public class GetQuestionDetailQuery
    {
        public class Request: IRequest<Response>
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid AnswerId { get; set; }
            public Guid? AuditionId { get; set; }
        }

        public class Handler: IRequestHandler<Request, Response>
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
                    .ProjectTo<Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

                if (question == null)
                {
                    return null;
                }

                question = _mapper.Map(request, question);

                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Id = question.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public Guid AnswerId { get; set; }
            public Guid? AuditionId { get; set; }
        }
    }
}
