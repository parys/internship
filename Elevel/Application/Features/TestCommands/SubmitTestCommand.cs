using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.GrammarAnswers)
                    .Must(x => x is null ? true : x.Count() == x.Distinct().Count())
                    .WithMessage("Some answers from grammar are the same");

                RuleFor(x => x.AuditionAnswers)
                    .Must(x => x is null ? true : x.Count() == x.Distinct().Count())
                    .WithMessage("Some answers from audition are the same");
            }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            private readonly UserManager<User> _userManager;

            private readonly IMailService _mailService;

            public Handler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager, IMailService mailService)
            {
                _context = context;

                _mapper = mapper;

                _userManager = userManager;

                _mailService = mailService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var test = await _context.Tests.FirstOrDefaultAsync(a => a.Id == request.Id, cancelationtoken);

                if (test is null)
                {
                    throw new NotFoundException($"test with {request.Id}", test);
                }

                if (request.UserId != test.UserId)
                {
                    throw new ValidationException("You can't submit this test");
                }

                if (request.EssayAnswer.Length > Constants.ESSAY_MAX_LENGTH)
                {
                    throw new ValidationException("Essay Answer is too long");
                }

                //if (DateTimeOffset.Compare(((DateTimeOffset)test.TestPassingDate).AddMinutes(Constants.TEST_DURATION), DateTimeOffset.Now) < 0)
                //{
                //    throw new ValidationException("Test time has passed");
                //}

                if (!request.GrammarAnswers.Any()
                    && !request.AuditionAnswers.Any())
                {
                    test.GrammarMark = Constants.MIN_MARK;
                    test.AuditionMark = Constants.MIN_MARK;

                }
                else
                {
                    var allAnswers = request.GrammarAnswers.Union(request.AuditionAnswers);

                    await CheckAnswersBelongtoTestAsync(allAnswers, test.Id);
                    CheckSingleAnswerForQuestion(allAnswers, test.Id);

                    test.GrammarMark = EvaluateTest(request.GrammarAnswers);
                    test.AuditionMark = EvaluateTest(request.AuditionAnswers);

                    await SaveAnswers(allAnswers);
                }
                
                test = _mapper.Map(request, test);

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

                var testResponse = _mapper.Map<Response>(test);

                var waitingAssignmentTestAmount = _context.Tests.Count(x => x.AuditionMark.HasValue && !x.CoachId.HasValue);

                foreach (var admin in await _userManager.GetUsersInRoleAsync(nameof(UserRole.Administrator)))
                {
                    _mailService.SendMessage(admin.Email,
                        "The test is submitted",
                        $"{waitingAssignmentTestAmount} Tests are waiting for assignment to coaches.<br/>"
                        + "Please go to the following link to assign them: <br/>"
                        + "<a href=\"http://exadel-train-app.herokuapp.com/adminProfile\">Assign the test</a><br/><br/>");
                }

                return testResponse;
            }

            private async Task CheckAnswersBelongtoTestAsync(IEnumerable<Guid> answers, Guid testId)
            {
                var questionIds = await _context.TestQuestions
                    .Where(x => x.TestId == testId)
                    .Join(_context.Answers, tq => tq.QuestionId, an => an.QuestionId,
                        (tq, an) => new
                        {
                            AnswerId = an.Id
                        })
                    .Select(x => x.AnswerId)
                    .ToListAsync();

                if (!answers.All(x => questionIds.Contains(x)))
                {
                    throw new ValidationException("There aren't some answers from current test");
                }
            }

            private void CheckSingleAnswerForQuestion(IEnumerable<Guid> answers, Guid testId)
            {
                var questionAnswers = _context.TestQuestions.Where(x => x.TestId == testId)
                    .Join(_context.Answers,
                    tq => tq.QuestionId,
                    an => an.QuestionId,
                    (tq, an) => new
                    {
                        QuestionId = an.QuestionId,
                        AnswerId = an.Id
                    })
                    .GroupBy(
                    x => x.QuestionId, (key, value) => new
                    {
                        QuestionId = key,
                        AnswersAmount = value
                        .Select(x => x.AnswerId)
                        .Where(x => answers.Contains(x))
                        .Count()
                    });


                if (questionAnswers.Any(x => x.AnswersAmount > Constants.ANSWERS_AMOUNT_PER_QUESTION))
                {
                    throw new ValidationException("There aren't some answers from te same question");
                }
            }

            private int EvaluateTest(IEnumerable<Guid> answers)
            {
                return _context.Answers
                    .Count(x => answers.Contains(x.Id) && x.IsRight);
            }

            private async Task SaveAnswers(IEnumerable<Guid> answers)
            {
                var testQuestions = await _context.TestQuestions
                    .Include(x => x.Question)
                    .ThenInclude(x => x.Answers)
                    .Where(x => x.Question.Answers
                        .Any(x => answers.Contains(x.Id)))
                    .ToListAsync();

                foreach (var testQuestion in testQuestions)
                {
                    testQuestion.UserAnswerId = testQuestion.Question.Answers
                        .FirstOrDefault(x => answers.Contains(x.Id)).Id;
                }
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