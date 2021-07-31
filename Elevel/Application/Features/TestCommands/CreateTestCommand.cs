using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            private const int AUDTUION_MIN_COUNT = 1;
            private const int TOPIC_MIN_COUNT = 2;

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
                if (auditions.Count < 1)
                {
                    throw new ValidationException("Not enough auditions");
                }

                var topics = await _context.Topics.AsNoTracking().Where(x => x.Level == request.Level).ToListAsync().ConfigureAwait(false);
                if (topics.Count < 2)
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

                var grammarQuestions = await CreateTestGrammarQuestionsAsync(test, cancelationtoken).ConfigureAwait(false);

                var auditionQuestions = await CreateTestAuditionQuestionsAsync(test, cancelationtoken).ConfigureAwait(false);

                var response = _mapper.Map<Response>(test);

                response.GrammarQuestions = await GetQuestionDtosAsync(response.Id, grammarQuestions)
                    .ConfigureAwait(false);
                response.Audition = await GetAuditionAsync(response.Id, (Guid)test.AuditionId, auditionQuestions)
                    .ConfigureAwait(false);
                response.Essay = await GetTopicAsync((Guid)test.EssayId)
                    .ConfigureAwait(false);
                response.Speaking = await GetTopicAsync((Guid)test.SpeakingId)
                    .ConfigureAwait(false);

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

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
            private List<TestQuestion> CreateTestQuestions(List<Guid> questionIds, Test test)
            {
                var testQuestions = new List<TestQuestion>();
                foreach (var question in questionIds)
                {
                    testQuestions.Add(new TestQuestion()
                    {
                        Id = Guid.NewGuid(),
                        TestId = test.Id,
                        QuestionId = question
                    });
                }
                _context.TestQuestions.AddRange(testQuestions);
                return testQuestions;
            }
            /// <summary>
            /// Generates Grammar test questions for received test
            /// </summary>
            /// <param name="test"></param>
            /// <param name="cancelationtoken"></param>
            /// <returns>true if not created, false if created</returns>
            private async Task<List<TestQuestion>> CreateTestGrammarQuestionsAsync(Test test, CancellationToken cancelationtoken)
            {
                var questions = await GetQuestionListAsync(test.Level);
                if (questions.Count() < GRAMMAR_TEST_COUNT)
                {
                    throw new ValidationException("Not Enough questions");
                }

                var questionIds = GetQuestionIds(questions, GRAMMAR_TEST_COUNT);

                var testQuestions = CreateTestQuestions(questionIds, test);

                return testQuestions;
            }
            /// <summary>
            /// Generates Audition test questions for received test
            /// </summary>
            /// <param name="test"></param>
            /// <param name="cancelationtoken"></param>
            /// <returns></returns>
            private async Task<List<TestQuestion>> CreateTestAuditionQuestionsAsync(Test test, CancellationToken cancelationtoken)
            {
                var questions = await GetQuestionListAsync(test.Level, test.AuditionId);

                if (questions.Count() < AUDITION_TEST_COUNT)
                {
                    throw new ValidationException("Not Enough questions");
                }

                var questionIds = GetQuestionIds(questions, AUDITION_TEST_COUNT);

                var testQuestions = CreateTestQuestions(questionIds, test);

                return testQuestions;
            }

            private async Task<IEnumerable<Question>> GetQuestionsByAuditionIdAsync(IEnumerable<TestQuestion> testQuestions, Guid? auditionId)
            { 
                var testQuestionIds = testQuestions.Select(x => x.QuestionId);
                return await _context.Questions.Where(x => x.AuditionId == auditionId && testQuestionIds.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
            }
            private async Task AddAnswersAsync(List<QuestionDto> questions)
            {
                var questionId = questions.Select(x => x.Id);
                var answerList = await _context.Answers.AsNoTracking().Where(x => questionId.Contains(x.QuestionId)).ToListAsync().ConfigureAwait(false);
                foreach (var question in questions)
                {
                    question.Answers = _mapper.Map<List<AnswerDto>>(answerList.Where(x => x.QuestionId == question.Id));
                }
            }
            
            private async Task<IEnumerable<QuestionDto>> GetQuestionDtosAsync(Guid testId, List<TestQuestion> testQuestions, Guid? auditionId = null)
            {
                var questions = await GetQuestionsByAuditionIdAsync(testQuestions, auditionId).ConfigureAwait(false);

                var questionDtos = _mapper.Map<List<QuestionDto>>(questions);

                await AddAnswersAsync(questionDtos).ConfigureAwait(false);

                return questionDtos;
            }
            private async Task<AuditionDto> GetAuditionAsync(Guid testId, Guid auditionId, List<TestQuestion> testQuestions)
            {
                var audition = await _context.Auditions
                    .ProjectTo<AuditionDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == auditionId).ConfigureAwait(false);
                audition.Questions = await GetQuestionDtosAsync(testId, testQuestions, auditionId).ConfigureAwait(false);
                return audition;
            }
            private async Task<TopicDto> GetTopicAsync(Guid topicId)
            {
                return await _context.Topics.ProjectTo<TopicDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == topicId).ConfigureAwait(false);
            }
        }

        public class Response
        {
            public Guid Id { get; set; }

            public long TestNumber { get; set; }

            public Level Level { get; set; }

            public Guid UserId { get; set; }

            public DateTimeOffset TestPassingDate { get; set; }

            public AuditionDto Audition { get; set; }

            public TopicDto Essay { get; set; }

            public TopicDto Speaking { get; set; }

            public IEnumerable<QuestionDto> GrammarQuestions { get; set; }

        }


        public class QuestionDto
        {
            public Guid Id { get; set; }
            public string NameQuestion { get; set; }
            public Guid? AuditionId { get; set; }
            public IEnumerable<AnswerDto> Answers { get; set; }
        }

        public class AuditionDto
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
            public IEnumerable<QuestionDto> Questions { get; set; }
        }

        public class AnswerDto
        {
            public Guid Id { get; set; }
            public string NameAnswer { get; set; }
        }
        public class TopicDto
        {
            public Guid Id { get; set; }
            public string TopicName { get; set; }
        }
    }
}