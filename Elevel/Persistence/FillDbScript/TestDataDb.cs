using Elevel.Application.Infrastructure;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Elevel.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        static TestDataDb()
        {
            Auditions = new List<Audition>();
            Topics = new List<Topic>();
            Questions = new List<Question>();
            Answers = new List<Answer>();
        }

        public static async Task FillDB(ApplicationDbContext context, UserManager<User> userManager)
        {
            Guid creatorId = (await userManager.GetUsersInRoleAsync(nameof(UserRole.Coach))).FirstOrDefault().Id;

            for (int del = 0; del < 2; del++)
            {
                for (int level = 1; level <= Constants.LEVEL_AMOUNT; level++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var audition = new Audition
                        {
                            Id = Guid.NewGuid(),
                            AudioFilePath = (level == 1 ? "A2.mp3"
                            : level == 2 ? "A2plus.mp3"
                            : level == 3 ? "B1.mp3"
                            : level == 4 ? "B2.mp3"
                            : "C1.mp3"),
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
                                NameQuestion = (questionNumber == 0 ? "A: You are ill. You ...... in the balcony. It isn't warm outside. B: I\'m wearing warm clothes. Don\'t worry."
                                : questionNumber == 1 ? "A: I don't want to leave my phone at the desk before I enter the exam hall.......? B: Unfortunately, yes.It is the rule."
                                : questionNumber == 2 ? "A: You ...... wash those strawberries I've already washed them B: Oh, good.Thank you. "
                                : questionNumber == 3 ? "I ........tell the time when I was 8 years old, but now I can. "
                                : questionNumber == 4 ? "A: ...... help you? B: Yes, please. I'm looking for a leather jacket. "
                                : questionNumber == 5 ? "A: Betty ...... pay her rent today. B: I hope she has enough money."
                                : questionNumber == 6 ? "Margaret ......... speak Italian and English, so she..........work with a translator in this project."
                                : questionNumber == 7 ? "A: I........open this jar. B: Let me help you. ....... use a knife ? A : No problem. "
                                : questionNumber == 8 ? "A: ...... turn on the volume of the radio 7 This is my favourite song.B: Sure.You......ask me.I'm never disturbed by music. "
                                : "A: ...... ask you a question ? B: Yes, sure, but I......answer it if it is about my private life."),
                                Level = (Level)level,
                                Deleted = deleted,
                                CreatorId = creatorId,
                                AuditionId = audition.Id
                            };

                            Questions.Add(question);

                            var qusetionAnswer = new List<Answer>
                            {
                                new Answer
                                {
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                        questionNumber == 0 ? "Needn't sit" :
                                        questionNumber == 1 ? "Can I" :
                                        questionNumber == 2 ? "Needn't" :
                                        questionNumber == 3 ? "Can" :
                                        questionNumber == 4 ? "Must I" :
                                        questionNumber == 5 ? "Can" :
                                        questionNumber == 6 ? "needn't / mustn't" :
                                        questionNumber == 7 ? "can't / Can I" :
                                        questionNumber == 8 ? " May I / can't " :
                                        "Do I / needn't"
                                    ),
                                    IsRight = (questionNumber == 2 || questionNumber == 7 ? true : false),
                                    QuestionId = question.Id
                                },
                                new Answer
                                {
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                        questionNumber == 0 ? "Can it" :
                                        questionNumber == 1 ? "Must I" :
                                        questionNumber == 2 ? "Must" :
                                        questionNumber == 3 ? "Couldn't" :
                                        questionNumber == 4 ? "Have I" :
                                        questionNumber == 5 ? "Must" :
                                        questionNumber == 6 ? "can't / couldn't" :
                                        questionNumber == 7 ? "mustn't / Must I" :
                                        questionNumber == 8 ? "Could I / must" :
                                        "Can I / needn't"
                                    ),
                                    IsRight = (questionNumber == 1 || questionNumber == 3 || questionNumber == 5 ? true : false),
                                    QuestionId = question.Id
                                },
                                new Answer
                                {
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                        questionNumber == 0 ? "Must sit" :
                                        questionNumber == 1 ? "May I" :
                                        questionNumber == 2 ? "Coudn't" :
                                        questionNumber == 3 ? "Can't" :
                                        questionNumber == 4 ? "Can I" :
                                        questionNumber == 5 ? "Needn't" :
                                        questionNumber == 6 ? "can / needn't" :
                                        questionNumber == 7 ? "needn't / May I" :
                                        questionNumber == 8 ? "Must I / could" :
                                        "May I / can't I"
                                    ),
                                    IsRight = (questionNumber == 4 || questionNumber == 6 || questionNumber == 9 ? true : false),
                                    QuestionId = question.Id
                                },
                                new Answer
                                {
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                        questionNumber == 0 ? "Mustn't sit" :
                                        questionNumber == 1 ? "Could I" :
                                        questionNumber == 2 ? "can" :
                                        questionNumber == 3 ? "could" :
                                        questionNumber == 4 ? "Did I" :
                                        questionNumber == 5 ? "Can't" :
                                        questionNumber == 6 ? "must / musn't" :
                                        questionNumber == 7 ? "can / Could I" :
                                        questionNumber == 8 ? "Can I/ needn't " :
                                        "Did I / could"
                                    ),
                                    IsRight = (questionNumber == 0 || questionNumber == 8 ? true : false),
                                    QuestionId = question.Id
                                }
                            };

                            Answers.AddRange(qusetionAnswer);
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        var topic = new Topic
                        {
                            Id = Guid.NewGuid(),
                            TopicName = (
                                i == 0 ? "Animals" :
                                i == 1 ? "Planes" :
                                i == 2 ? "Sandwich recipe" :
                                i == 3 ? "Space" : 
                                "Energetics"
                                ),
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
                                NameQuestion = (
                                            questionNumber == 0 ? "He had travelled only twenty miles into the desert" +
                                            "when his vehicle developed engine trouble.There" +
                                            "was no immediate help available and he had to..............from the race." :

                                            questionNumber == 1 ? "............ ocean-liners offer their voyagers a great" +
                                            "variety of civilized comforts. " :

                                            questionNumber == 2 ? " Aggressiveness, which we may think of as the violent" +
                                            "expression of extreme selfishness, is relatively simple" +
                                            "to explain in evolutionary..............." :

                                            questionNumber == 3 ? " I'm ............... of seeing your stupid face around all" +
                                            "the time. " :

                                            questionNumber == 4 ? "They say a woman might forgive an insult; but she" +
                                            "would never forgive being............... ." :

                                            questionNumber == 5 ? "These results seem to ............... that calcium and" +
                                            "vitamin D supplementation may also prevent tooth" +
                                            "loss from gum disease. " :

                                            questionNumber == 6 ? " He would have been ............... if the President's" +
                                            "pardon hadn't arrived just in the nick of time. " :

                                            questionNumber == 7 ? "He is a rather timid person on the whole. But, when" +
                                            "he has had a few drinks," +
                                            "he becomes quite.............. . " :

                                            questionNumber == 8 ? "The generation ............... seems to be getting bigger" +
                                            "and bigger in our day and age." :

                                            questionNumber == 9 ? "Am I to ............... from your last remarks that my" +
                                            "services are no longer required here ?" :

                                            questionNumber == 10 ? " I'm going to ask you a number of questions, and I" +
                                            "want...............answers from you." :

                                            " I'm going to ask you a number of questions, and I want............... answers from you."
                                        ),
                                Level = (Level)level,
                                Deleted = deleted,
                                CreatorId = creatorId
                            };

                            Questions.Add(question);

                            var qusetionAnswer = new List<Answer>()
                            {
                                new Answer{
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                            questionNumber == 0 ? "retreat" :
                                            questionNumber == 1 ? "Vulgar" :
                                            questionNumber == 2 ? "texts" :
                                            questionNumber == 3 ? "sick and tired" :
                                            questionNumber == 4 ? "sacrificed" :
                                            questionNumber == 5 ? "define" :
                                            questionNumber == 6 ? "made a hangman" :
                                            questionNumber == 7 ? "impressive" :
                                            questionNumber == 8 ? "division" :
                                            questionNumber == 9 ? "distrust" :
                                            questionNumber == 10 ? "reluctant" :
                                            "receive from"
                                        ),
                                    IsRight = (questionNumber == 3 ? true : false),
                                    QuestionId = question.Id
                                },
                                new Answer{
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                            questionNumber == 0 ? "detour" :
                                            questionNumber == 1 ? "Competent" :
                                            questionNumber == 2 ? "books" :
                                            questionNumber == 3 ? "loving and found" :
                                            questionNumber == 4 ? "persuaded" :
                                            questionNumber == 5 ? "imply" :
                                            questionNumber == 6 ? "elevated" :
                                            questionNumber == 7 ? "permissive" :
                                            questionNumber == 8 ? "partition" :
                                            questionNumber == 9 ? "refer" :
                                            questionNumber == 10 ? "obstinate" :
                                            "offer"
                                        ),
                                    IsRight = (questionNumber == 5 ? true : false),
                                    QuestionId = question.Id
                                },
                                new Answer{
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                            questionNumber == 0 ? "deviate" :
                                            questionNumber == 1 ? "Prospective" :
                                            questionNumber == 2 ? "words" :
                                            questionNumber == 3 ? "jealous and angry" :
                                            questionNumber == 4 ? "ignored" :
                                            questionNumber == 5 ? "refer" :
                                            questionNumber == 6 ? "executed" :
                                            questionNumber == 7 ? "aggressive" :
                                            questionNumber == 8 ? "gap" :
                                            questionNumber == 9 ? "infer" :
                                            questionNumber == 10 ? "straight" :
                                            "risk to"
                                        ),
                                    IsRight = (questionNumber == 4 ||
                                               questionNumber == 6 ||
                                               questionNumber == 7 ||
                                               questionNumber == 8 ||
                                               questionNumber == 9 ||
                                               questionNumber == 10 ? true : false),
                                    QuestionId = question.Id
                                },
                                new Answer{
                                    Id = Guid.NewGuid(),
                                    NameAnswer = (
                                            questionNumber == 0 ? "withdraw" :
                                            questionNumber == 1 ? "Contemporary" :
                                            questionNumber == 2 ? "terms" :
                                            questionNumber == 3 ? "displeased and disappointed " :
                                            questionNumber == 4 ? "praised" :
                                            questionNumber == 5 ? "infer" :
                                            questionNumber == 6 ? "released" :
                                            questionNumber == 7 ? "intensive" :
                                            questionNumber == 8 ? "separation" :
                                            questionNumber == 9 ? "imply" :
                                            questionNumber == 10 ? "remarkable" :
                                            "bet"
                                        ),
                                    IsRight = (questionNumber == 0 ||
                                               questionNumber == 1 ||
                                               questionNumber == 2 ||
                                               questionNumber == 11 ? true : false),
                                    QuestionId = question.Id
                                }
                            };

                            Answers.AddRange(qusetionAnswer);
                        }
                    }
                }

                deleted = !deleted;
            }

            if (!await context.Topics.AnyAsync())
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
