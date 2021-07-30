﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class GetTestByIdQuery
    {
        public class Request: IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var test = await _context.Tests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                if(test == null)
                {
                    throw new NotFoundException($"Test with ID: {request.Id}");
                }
                var response = _mapper.Map<Response>(test);
                response.GrammarQuestions = await GetQuestionDtosAsync(response.Id)
                    .ConfigureAwait(false);
                response.Audition = await GetAuditionAsync(response.Id, (Guid)test.AuditionId)
                    .ConfigureAwait(false);
                response.Essay = await GetTopicAsync((Guid)test.EssayId)
                    .ConfigureAwait(false);
                response.Speaking = await GetTopicAsync((Guid)test.SpeakingId)
                    .ConfigureAwait(false);

                return response;
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
            private async Task<IEnumerable<QuestionDto>> GetQuestionDtosAsync(Guid testId, Guid? auditionId = null)
            {
                var testQuestions = await _context.TestQuestions.Where(x => x.TestId == testId).ToListAsync().ConfigureAwait(false);

                var questions = await GetQuestionsByAuditionIdAsync(testQuestions, auditionId).ConfigureAwait(false);

                var questionDtos = _mapper.Map <List<QuestionDto>>(questions);

                await AddAnswersAsync(questionDtos).ConfigureAwait(false);

                return questionDtos;
            }
            private async Task<AuditionDto> GetAuditionAsync(Guid testId, Guid auditionId)
            {
                var audition = await _context.Auditions
                    .ProjectTo<AuditionDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == auditionId).ConfigureAwait(false);
                audition.Questions = await GetQuestionDtosAsync(testId, auditionId).ConfigureAwait(false);
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
