using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class SubmitTestCommand
    {
        public class Request : IRequest<Response>
        {
            [JsonIgnore]
            public Guid Id { get; set; }

            [JsonIgnore]
            public Guid UserId { get; set; }

            public IEnumerable<Guid> GrammarAnswers { get; set; }

            public IEnumerable<Guid> AuditionAnswers { get; set; }

            public string EssayAnswer { get; set; }

            public string SpeakingAnswerReference { get; set; }

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
                var test = await _context.Tests.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);

                if (test is null)
                {
                    throw new NotFoundException($"test with {request.Id}", test);
                }

                if(request.UserId != test.UserId)
                {
                    throw new ValidationException("You can't submit this test");
                }

                if (request.EssayAnswer.Length > Constants.ESSAY_MAX_LENGTH)
                {
                    throw new ValidationException("Essay Answer is too long");
                }

                if (DateTimeOffset.Compare(((DateTimeOffset)test.TestPassingDate).AddMinutes(Constants.TEST_DURATION), DateTimeOffset.Now) < 0)
                {
                    throw new ValidationException("Test time has passed");
                }

                await CheckAnswersForUniqueTestQuestionAsync(request.GrammarAnswers, test.Id);

                await CheckAnswersForUniqueTestQuestionAsync(request.AuditionAnswers, test.Id);

                test = _mapper.Map(request, test);

                test.GrammarMark = await EvaluateTestAsync(request.GrammarAnswers);

                test.AuditionMark = await EvaluateTestAsync(request.AuditionAnswers);

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

                var testResponse = _mapper.Map<Response>(test);

                return testResponse;
            }

            private async Task CheckAnswersForUniqueTestQuestionAsync(IEnumerable<Guid> answers, Guid testId)
            {
                var questionIds = await _context.TestQuestions.Where(x => x.TestId == testId).Select(x => x.QuestionId).ToListAsync();
                var answerList = _context.Answers.AsNoTracking().Where(x => questionIds.Contains(x.QuestionId));

                foreach (var answer in answers)
                {
                    if (!await answerList.AnyAsync(x => x.Id == answer).ConfigureAwait(false))
                    {
                        throw new ValidationException($"Answer with Id {answer} is not in current test");
                    }
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
    }
}