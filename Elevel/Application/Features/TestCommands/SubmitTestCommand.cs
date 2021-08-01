using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class SubmitTestCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }

            public IEnumerable<Guid> GrammarAnswers { get; set; }

            public IEnumerable<Guid> AuditionAnswers { get; set; }

            public string EssayAnswer { get; set; }

            public string SpeakingAnswerReference { get; set; }

        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private const int GRAMMAR_QUESTION_COUNT = 12;
            private const int AUDITION_QUESTION_COUNT = 10;
            private const int ESSAY_MAX_LENGTH = 512;
            private const int TEST_DURATION = 60; //minutes

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var test = await _context.Tests.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken).ConfigureAwait(false);

                if (test is null)
                {
                    throw new NotFoundException($"test with {request.Id}", test);
                }

                if (request.EssayAnswer.Length > ESSAY_MAX_LENGTH)
                {
                    throw new ValidationException("Essay Answer is too long");
                }

                if (DateTimeOffset.Compare(((DateTimeOffset)test.TestPassingDate).AddMinutes(TEST_DURATION), DateTimeOffset.Now) < 0)
                {
                    throw new ValidationException("Test time has passed");
                }

                await CheckAnswersForUniqueQuestionAsync(request.GrammarAnswers, GRAMMAR_QUESTION_COUNT);

                await CheckAnswersForUniqueQuestionAsync(request.AuditionAnswers, AUDITION_QUESTION_COUNT);

                test = _mapper.Map<Test>(request);

                test.GrammarMark = await EvaluateTestAsync(request.GrammarAnswers).ConfigureAwait(false);

                test.AuditionMark = await EvaluateTestAsync(request.AuditionAnswers).ConfigureAwait(false);

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

                var testResponse = _mapper.Map<Response>(test);

                return testResponse;
            }

            private async Task CheckAnswersForUniqueQuestionAsync(IEnumerable<Guid> answers, int count)
            {
                var answerList = _context.Answers.AsNoTracking();

                foreach (var answer in answers)
                {
                    if (!await answerList.AnyAsync(x => x.Id == answer).ConfigureAwait(false))
                    {
                        throw new NotFoundException($"Answer with Id {answer}");
                    }
                }

                var questionIds = await answerList
                    .Where(x => answers.Contains(x.Id))
                    .Select(x => x.QuestionId).Distinct()
                    .ToListAsync().ConfigureAwait(false);

                if (questionIds.Count != count)
                {
                    throw new ValidationException("You wrote several answers from one question");
                }
            }

            private Task<int> EvaluateTestAsync(IEnumerable<Guid> answers)
            {
                return Task.FromResult(_context.Answers.Where(x => answers.Contains(x.Id) && x.IsRight).Count());
            }

        }

        public class Response
        {
            public Guid Id { get; set; }
            public Level Level { get; set; }

            public DateTimeOffset TestPassingDate { get; set; }

            public int GrammarMark { get; set; }
            public int AuditionMark { get; set; }

            public Guid UserId { get; set; }
        }


        public class AnswersDto
        {
            public Guid AnswerId { get; set; }
        }
    }
}