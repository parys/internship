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
            CreateMap<Question, DeleteQuestionCommand.Response>();
            CreateMap<Question, GetQuestionDetailQuery.Response>();
            CreateMap<Question, GetQuestionListQuery.QuestionsDTO>();
            CreateMap<Question, UpdateQuestionCommand.Response>();
            CreateMap<UpdateQuestionCommand.Request, Question>();
        }
    }
}
