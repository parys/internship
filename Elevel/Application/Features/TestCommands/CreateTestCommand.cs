﻿using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId).ConfigureAwait(false) == null)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "UserId is not found"
                    };
                }

                var Hr = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.HrId).ConfigureAwait(false);

                if (request.HrId.HasValue
                    && Hr == null)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "HrId is not found"
                    };
                }
                if (!(await _userManager.GetRolesAsync(Hr)).Contains("HumanResourceManager"))
                {
                    return new Response()
                    {
                        Id = null,
                        isHr = true
                    };
                }
                if(request.HrId == request.UserId)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "You can't assign yourself"
                    };
                }
                

                

                var v = await _context.Tests.FirstOrDefaultAsync(x => 0 == DateTimeOffset.Compare(x.CreationDate.Date, DateTimeOffset.UtcNow));


                if (!request.HrId.HasValue && v != null)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "You already had a test today"
                    };
                }

                if (request.AssignmentEndDate.Date < DateTimeOffset.UtcNow.Date)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "assignmentEndDate can't be in the past"
                    };
                }

                var test = _mapper.Map<Test>(request);

                var auditions = await _context.Auditions.AsNoTracking().Where(x => x.Level == request.Level && !x.Deleted).ToListAsync().ConfigureAwait(false);

                if (auditions.Count() < 1)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "Not enough auditions"
                    };
                }

                var topics = await _context.Topics.AsNoTracking().Where(x => x.Level == request.Level && !x.Deleted).ToListAsync().ConfigureAwait(false);

                if (topics.Count() < 2)
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "Not enough topics"
                    };
                }

                test.Id = Guid.NewGuid();
                test.EssayId = SetEssay(topics);
                test.SpeakingId = SetSpeaking(topics, test);
                test.AuditionId = SetAudition(auditions);

                _context.Tests.Add(test);

                if (await CreateTestGrammarQuestionsAsync(test, cancelationtoken).ConfigureAwait(false))
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "Not enough Grammar Questions"
                    };
                }
                if (await CreateTestAuditionQuestionsAsync(test, cancelationtoken).ConfigureAwait(false))
                {
                    return new Response()
                    {
                        Id = null,
                        Message = "Not enough Audition Questions"
                    };
                }

                await _context.SaveChangesAsync(cancelationtoken).ConfigureAwait(false);

                return new Response { Id = test.Id };
            }

            private Guid? SetEssay(IEnumerable<Topic> topics)
            {
                return topics
                .ElementAt(_rand.Next(0, topics.Count() - 1))
                .Id;
            }

            private Guid? SetSpeaking(IEnumerable<Topic> topics, Test test)
            {
                var filteredTopics = topics.Where(x => x.Id != test.EssayId);
                return filteredTopics.ElementAt(_rand.Next(0, topics.Count() - 1)).Id;
            }

            private Guid? SetAudition(IEnumerable<Audition> auditions)
            {
                return auditions
                .ElementAt(_rand.Next(0, auditions.Count() - 1))
                .Id;
            }

            private async Task<bool> CreateTestGrammarQuestionsAsync(Test test, CancellationToken cancelationtoken)
            {
                var questions = await _context
                .Questions
                .AsNoTracking()
                .Where(x => x.Level == test.Level && x.AuditionId == null && !x.Deleted).ToListAsync().ConfigureAwait(false);
                if (questions.Count() < 12)
                {
                    return true;
                }

                var questionIds = new List<Guid>();


                for (int i = 0; i < GRAMMAR_TEST_COUNT; i++)
                {
                    var filteredQuestions = questions.Where(x => !questionIds.Contains(x.Id));
                    questionIds.Add(filteredQuestions.ElementAt(_rand.Next(0, filteredQuestions.Count() - 1)).Id); ;
                }

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
                return false;
            }

            private async Task<bool> CreateTestAuditionQuestionsAsync(Test test, CancellationToken cancelationtoken)
            {
                var questions = await _context
                .Questions
                .AsNoTracking()
                .Where(x => x.Level == test.Level && x.AuditionId == test.AuditionId && !x.Deleted).ToListAsync().ConfigureAwait(false);

                if (questions.Count() < 10)
                {
                    return true;
                }

                var questionIds = new List<Guid>();

                for (int i = 0; i < AUDITION_TEST_COUNT; i++)
                {
                    var filteredQuestions = questions
                        .Where(x => !questionIds.Contains(x.Id));
                    questionIds.Add(filteredQuestions.ElementAt(_rand.Next(0, filteredQuestions.Count() - 1)).Id);
                }

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
                return false;
            }
        }

        public class Response
        {
            public Guid? Id { get; set; }
            public string Message { get; set; }
            public bool? isHr { get; set; }
        }
    }
}