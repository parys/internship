using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }
            public async Task<Response> Handle(Request request, CancellationToken cancelationtoken)
            {
                var test = _mapper.Map<Test>(request);

                var auditions = _context.Auditions.AsNoTracking().Where(x => x.Level == request.Level);
                var topics = _context.Topics.AsNoTracking().Where(x => x.Level == request.Level);

                test.EssayId = SetEssay( topics);

                test.SpeakingId = SetSpeaking( topics, test);

                test.AuditionId = SetAudition( auditions);

                CreateTestGrammarQuestions(test, cancelationtoken);

                CreateTestAuditionQuestions(test, cancelationtoken);

                _context.Tests.Add(test);
                await _context.SaveChangesAsync(cancelationtoken);

                return new Response { Id = test.Id };
            }

            public Guid SetEssay(IQueryable<Topic> topics)
            {
                return topics
                    .ElementAt(_rand.Next(0, topics.Count()))
                    .Id;
            }

            public Guid SetSpeaking(IQueryable<Topic> topics, Test test)
            {
                return topics
                    .Where(x => x.Id != test.EssayId)
                    .ElementAt(_rand.Next(0, topics.Count()))
                    .Id;
            }

            public Guid SetAudition(IQueryable<Audition> auditions)
            {
                return auditions
                    .ElementAt(_rand.Next(0, auditions.Count()))
                    .Id;
            }

            public async void CreateTestGrammarQuestions(Test test, CancellationToken cancelationtoken)
            {
                var questions = _context
                    .Questions
                    .AsNoTracking()
                    .Where(x => x.Level == test.Level && x.AuditionId == null).Take(12);

                foreach (var question in questions)
                {
                    var testQuestion = new TestQuestion()
                    {
                        Id = Guid.NewGuid(),
                        TestId = test.Id,
                        QuestionId = question.Id
                    };
                    _context.TestQuestions.Add(testQuestion);
                    await _context.SaveChangesAsync(cancelationtoken);
                }
            }

            public async void CreateTestAuditionQuestions(Test test, CancellationToken cancelationtoken)
            {
                var questions = _context
                    .Questions
                    .AsNoTracking()
                    .Where(x => x.Level == test.Level && x.AuditionId == test.AuditionId).Take(10);

                foreach (var question in questions)
                {
                    var testQuestion = new TestQuestion()
                    {
                        Id = Guid.NewGuid(),
                        TestId = test.Id,
                        QuestionId = question.Id
                    };
                    _context.TestQuestions.Add(testQuestion);
                    await _context.SaveChangesAsync(cancelationtoken);
                }
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}