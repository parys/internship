using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class CreateTestCommand
    {
        public class Request : IRequest<Response>
        {
            public Level Level { get; set; }
            public Guid UserId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private static Random _rand = new Random();
            private readonly UserManager<ApplicationUser> _userManager;

            private const int GRAMMAR_TEST_COUNT = 12;
            private const int AUDITION_TEST_COUNT = 10;
            public Handler(IApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;

            }
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                if (!await _userManager.Users.AnyAsync(x => x.Id == request.UserId))
                {
                    throw new NotFoundException($"User with {request.UserId}");
                }

                if (await _context.Tests.AnyAsync(x => x.UserId == request.UserId
                    && DateTimeOffset.Compare(x.CreationDate.Date, DateTimeOffset.UtcNow.Date) == 0))
                {
                    throw new ValidationException($"User: {request.UserId} has already had a test today");
                }

                var test = _mapper.Map<Test>(request);

                var auditions = await _context.Auditions.AsNoTracking().Where(x => x.Level == request.Level).ToListAsync().ConfigureAwait(false);
                if (auditions.Count() < 1)
                {
                    throw new ValidationException("Not enough auditions");
                }

                var topics = await _context.Topics.AsNoTracking().Where(x => x.Level == request.Level).ToListAsync().ConfigureAwait(false);
                if (topics.Count() < 2)
                {
                    throw new ValidationException("Not enough topics");
                }

                test.Id = Guid.NewGuid();
                test.EssayId = FindTopic(topics);
                test.SpeakingId = FindTopic(topics, test.EssayId);
                test.AuditionId = FindAudition(auditions);
                test.TestPassingDate = DateTimeOffset.UtcNow;
                test.AssignmentEndDate = null;

                _context.Tests.Add(test);

                if (await CreateTestGrammarQuestionsAsync(test, cancelationtoken).ConfigureAwait(false))
                {
                    throw new ValidationException("Not enough Grammar Questions");
                }
                if (await CreateTestAuditionQuestionsAsync(test, cancelationtoken).ConfigureAwait(false))
                {
                    throw new ValidationException("Not enough Audition Questions");
                }

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

                var response = _mapper.Map<Response>(test);

                return response;
            }

            /// <summary>
            /// Finds a topic guid
            /// </summary>
            /// <param name="topics"></param>
            /// <param name="test"></param>
            /// <returns></returns>
            private Guid? FindTopic(IEnumerable<Topic> topics, Guid? EssayId = null)
            {
                var filteredTopics = topics.Where(x => x.Id != EssayId);
                return filteredTopics.ElementAt(_rand.Next(0, topics.Count() - 1)).Id;
            }
            /// <summary>
            /// Finds an audition
            /// </summary>
            /// <param name="auditions"></param>
            /// <returns></returns>
            private Guid? FindAudition(IEnumerable<Audition> auditions)
            {
                return auditions
                .ElementAt(_rand.Next(0, auditions.Count() - 1))
                .Id;
            }
            /// <summary>
            /// Async method returns a collection of questions based on level
            /// </summary>
            /// <param name="level"></param>
            /// <param name="AuditionId">null default</param>
            /// <returns>Task<IEnumerable<Question>></returns>
            private async Task<IEnumerable<Question>> GetQuestionListAsync(Level level, Guid? AuditionId = null)
            {
                return await _context
                .Questions
                .AsNoTracking()
                .Where(x => x.Level == level && x.AuditionId == AuditionId).ToListAsync().ConfigureAwait(false);
            }
            /// <summary>
            /// Returns a list of {count} random QuestionId from received questions
            /// </summary>
            /// <param name="questions"> IEnumerable<Question> </param>
            /// <returns>List<Guid></returns>
            private List<Guid> GetQuestionIds(IEnumerable<Question> questions, int count)
            {
                var questionIds = new List<Guid>();

                for (int i = 0; i < count; i++)
                {
                    var filteredQuestions = questions.Where(x => !questionIds.Contains(x.Id));
                    questionIds.Add(filteredQuestions.ElementAt(_rand.Next(0, filteredQuestions.Count() - 1)).Id); ;
                }
                return questionIds;
            }
            /// <summary>
            /// Creates test questions for test
            /// </summary>
            /// <param name="questionIds"></param>
            /// <param name="test"></param>
            private void CreateTestQuestionsForGrammar(List<Guid> questionIds, Test test)
            {
                foreach (var question in questionIds)
                {
                    var testQuestion = new TestQuestion()
                    {
                        Id = Guid.NewGuid(),
                        TestId = test.Id,
                        QuestionId = question
                    };
                    _context.TestQuestions.Add(testQuestion);
                }
            }
            /// <summary>
            /// Generates Grammar test questions for received test
            /// </summary>
            /// <param name="test"></param>
            /// <param name="cancelationtoken"></param>
            /// <returns>true if not created, false if created</returns>
            private async Task<bool> CreateTestGrammarQuestionsAsync(Test test, CancellationToken cancelationtoken)
            {
                var questions = await GetQuestionListAsync(test.Level);
                if (questions.Count() < 12)
                {
                    return true;
                }

                var questionIds = GetQuestionIds(questions, GRAMMAR_TEST_COUNT);

                CreateTestQuestionsForGrammar(questionIds, test);

                return false;
            }
            /// <summary>
            /// Generates Audition test questions for received test
            /// </summary>
            /// <param name="test"></param>
            /// <param name="cancelationtoken"></param>
            /// <returns></returns>
            private async Task<bool> CreateTestAuditionQuestionsAsync(Test test, CancellationToken cancelationtoken)
            {
                var questions = await GetQuestionListAsync(test.Level, test.AuditionId);

                if (questions.Count() < 10)
                {
                    return true;
                }

                var questionIds = GetQuestionIds(questions, AUDITION_TEST_COUNT);

                CreateTestQuestionsForGrammar(questionIds, test);

                return false;
            }
        }

        public class Response
        {
            public Guid Id { get; set; }

            public long TestNumber { get; set; }

            public Level Level { get; set; }

            public Guid UserId { get; set; }

            public DateTimeOffset TestPassingDate { get; set; }

            public Guid AuditionId { get; set; }

            public Guid EssayId { get; set; }

            public Guid SpeakingId { get; set; }
        }
    }
}