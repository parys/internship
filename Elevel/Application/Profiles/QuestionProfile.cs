using AutoMapper;
using Elevel.Application.Features.QuestionCommands;
using Elevel.Domain.Models;

namespace Elevel.Application.Profiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<CreateQuestionCommand.Request, Question>();
            CreateMap<CreateQuestionCommand.AnswerDto, Answer>();
            CreateMap<Question, CreateQuestionCommand.Response>();

            CreateMap<Question, DeleteQuestionCommand.Response>();

            CreateMap<Question, GetQuestionDetailQuery.Response>();
            CreateMap<Answer, GetQuestionDetailQuery.AnswerDto>();

            CreateMap<Question, GetQuestionListQuery.QuestionsDTO>();

            CreateMap<Question, UpdateQuestionCommand.Response>();
            CreateMap<Answer, UpdateQuestionCommand.AnswerDto>();
            CreateMap<UpdateQuestionCommand.Request, Question>();
            CreateMap<UpdateQuestionCommand.AnswerDto, Answer>();
        }
    }
}
