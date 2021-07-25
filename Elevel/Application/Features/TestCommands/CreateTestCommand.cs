using AutoMapper;
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
    public class CreateTestCommand
    {
        public class Request : IRequest<Response>
        {
            public Level Level { get; set; }
            public DateTimeOffset AssignmentEndDate { get; set; }
            public Guid UserId { get; set; }
            public Guid? HrId { get; set; }
            public bool Priority { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private static Random _rand = new Random();

            private const int GRAMMAR_TEST_COUNT = 12;
            private const int AUDITION_TEST_COUNT = 10;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var test = _mapper.Map<Test>(request);

                var auditions = await _context.Auditions.AsNoTracking().Where(x => x.Level == request.Level /*&& !x.Deleted*/).ToListAsync().ConfigureAwait(false); // --------------------------------------------------
                var topics = await _context.Topics.AsNoTracking().Where(x => x.Level == request.Level && !x.Deleted).ToListAsync().ConfigureAwait(false);

                test.Id = Guid.NewGuid();
                test.CreationDate = DateTimeOffset.UtcNow;
                test.EssayId = SetEssay(topics);
                test.SpeakingId = SetSpeaking(topics, test);
                test.AuditionId = SetAudition(auditions);

                _context.Tests.Add(test);

                await CreateTestGrammarQuestions(test, cancelationtoken).ConfigureAwait(false);
                await CreateTestAuditionQuestions(test, cancelationtoken).ConfigureAwait(false);

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

                return new Response { Id = test.Id };
            }

            private Guid? SetEssay(IEnumerable<Topic> topics)
            {
                if (topics.Count() == 0)
                {
                    return null;
                }
                return topics
                .ElementAt(_rand.Next(0, topics.Count()-1))
                .Id;
            }

            private Guid? SetSpeaking(IEnumerable<Topic> topics, Test test)
            {
                var filteredTopics = topics.Where(x => x.Id != test.EssayId);

                if (filteredTopics.Count() == 0)
                {
                    return null;
                }

                return filteredTopics.ElementAt(_rand.Next(0, topics.Count()-1)).Id;
            }

            private Guid? SetAudition(IEnumerable<Audition> auditions)
            {
                if (auditions.Count() == 0)
                {
                    return null;
                }
                return auditions
                .ElementAt(_rand.Next(0, auditions.Count()-1))
                .Id;
            }

            private async Task CreateTestGrammarQuestions(Test test, CancellationToken cancelationtoken)
            {
                var questions = await _context
                .Questions
                .AsNoTracking()
                .Where(x => x.Level == test.Level && x.AuditionId == null && !x.Deleted).ToListAsync().ConfigureAwait(false);

                var questionIds = new List<Guid>();

                for (int i = 0; i < GRAMMAR_TEST_COUNT; i++)
                {
                    questionIds.Add(questions.ElementAt(_rand.Next(0, questions.Count()-1)).Id);
                }

                foreach (var question in questions)
                {
                    var testQuestion = new TestQuestion()
                    {
                        Id = Guid.NewGuid(),
                        TestId = test.Id,
                        QuestionId = question.Id
                    };
                    _context.TestQuestions.Add(testQuestion);
                }
            }

            private async Task CreateTestAuditionQuestions(Test test, CancellationToken cancelationtoken)
            {
                var questions = await _context
                .Questions
                .AsNoTracking()
                .Where(x => x.Level == test.Level && x.AuditionId == test.AuditionId && !x.Deleted).ToListAsync().ConfigureAwait(false);

                var questionIds = new List<Guid>();

                for (int i = 0; i < AUDITION_TEST_COUNT; i++)
                {
                    questionIds.Add(questions.ElementAt(_rand.Next(0, questions.Count()-1)).Id);
                }

                foreach (var question in questions)
                {
                    var testQuestion = new TestQuestion()
                    {
                        Id = Guid.NewGuid(),
                        TestId = test.Id,
                        QuestionId = question.Id
                    };
                    _context.TestQuestions.Add(testQuestion);
                }
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}
    