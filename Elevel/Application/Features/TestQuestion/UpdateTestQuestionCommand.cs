using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestQuestion
{
    public class UpdateTestQuestionCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid? UserAnswerId { get; set; }
            public Guid TestId { get; set; }
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

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var answerUser =
                    await _context.TestQuestions.FirstOrDefaultAsync(x => x.TestId == request.TestId && x.QuestionId == request.QuestionId, cancellationToken: cancellationToken);

                if (answerUser == null)
                {
                    //Create myself exeption
                    return null;
                }

                answerUser = _mapper.Map(request, answerUser);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Id = answerUser.UserAnswerId };
            }
        }

        public class Response
        {
            public Guid? Id { get; set; }
        }
    }
}