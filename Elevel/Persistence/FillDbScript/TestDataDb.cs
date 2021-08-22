using Elevel.Application.Infrastructure;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Elevel.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Persistence.FillDbScript
{
    public static class TestDataDb
    {
        public static List<Audition> Auditions { get; set; }

        public static List<Topic> Topics { get; set; }

        public static List<Question> Questions { get; set; }

        public static List<Answer> Answers { get; set; }

        private static bool deleted = false;

        public static int rightNumber;
        static TestDataDb()
        {
            Auditions = new List<Audition>();
            Topics = new List<Topic>();
            Questions = new List<Question>();
            Answers = new List<Answer>();
            rightNumber = 0;
        }

        public static async Task FillDB(ApplicationDbContext context, UserManager<User> userManager)
        {
            Guid creatorId = (await userManager.Users.FirstOrDefaultAsync(x => x.FirstName == "Coach")).Id;

            for (int del = 0; del < 2; del++)
            {
                for (int level = 1; level <= Constants.LEVEL_AMOUNT; level++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var audition = new Audition
                        {
                            Id = Guid.NewGuid(),
                            AudioFilePath = "FilePath",
                            Level = (Level)level,
                            Deleted = deleted,
                            CreatorId = creatorId
                        };

                        Auditions.Add(audition);

                        for (int questionNumber = 0; questionNumber < Constants.AUDITION_QUESTION_AMOUNT; questionNumber++)
                        {
                            var question = new Question
                            {
                                Id = Guid.NewGuid(),
                                NameQuestion = $"Qusetion {i}{del}{level}{questionNumber}",
                                Level = (Level)level,
                                Deleted = deleted,
                                CreatorId = creatorId,
                                AuditionId = audition.Id
                            };

                            Questions.Add(question);

                            for (int answerNumber = 0; answerNumber < Constants.ANSWER_AMOUNT; answerNumber++)
                            {
                                var qusetionAnswer = new Answer
                                {
                                    Id = Guid.NewGuid(),
                                    NameAnswer = $"{(rightNumber == answerNumber ? "Right" : "Wrong")} answer {i}{del}{level}{questionNumber}{answerNumber}",
                                    IsRight = rightNumber == answerNumber ? true : false,
                                    QuestionId = question.Id
                                };

                                Answers.Add(qusetionAnswer);
                            }

                            ++rightNumber;
                            rightNumber = rightNumber % 4;
                        }
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        var topic = new Topic
                        {
                            Id = Guid.NewGuid(),
                            TopicName = $"topic {i}{del}{level}",
                            Level = (Level)level,
                            CreatorId = creatorId,
                            Deleted = deleted
                        };

                        Topics.Add(topic);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int questionNumber = 0; questionNumber < Constants.GRAMMAR_QUESTION_AMOUNT; questionNumber++)
                        {
                            var question = new Question
                            {
                                Id = Guid.NewGuid(),
                                NameQuestion = $"Qusetion {i}{del}{level}{questionNumber}",
                                Level = (Level)level,
                                Deleted = deleted,
                                CreatorId = creatorId
                            };

                            Questions.Add(question);

                            for (int answerNumber = 0; answerNumber < Constants.ANSWER_AMOUNT; answerNumber++)
                            {
                                var qusetionAnswer = new Answer
                                {
                                    Id = Guid.NewGuid(),
                                    NameAnswer = $"{(rightNumber == answerNumber ? "Right" : "Wrong")} answer {i}{del}{level}{questionNumber}{answerNumber}",
                                    IsRight = rightNumber == answerNumber ? true : false,
                                    QuestionId = question.Id
                                };

                                Answers.Add(qusetionAnswer);
                            }

                            ++rightNumber;
                            rightNumber = rightNumber % 4;
                        }
                    }
                }

                deleted = !deleted;
            }

            if(!await context.Topics.AnyAsync()) 
            {
                context.Topics.AddRange(Topics);
            }

            if (!await context.Auditions.AnyAsync())
            {
                context.Auditions.AddRange(Auditions);
            }

            if (!await context.Questions.AnyAsync())
            {
                context.Questions.AddRange(Questions);
            }

            if (!await context.Answers.AnyAsync())
            {
                context.Answers.AddRange(Answers);
            }

            await context.SaveChangesAsync();
        }
    }
}
