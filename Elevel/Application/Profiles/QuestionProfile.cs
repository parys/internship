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
            CreateMap<User, GetQuestionListQuery.QuestionsDTO>()
                .ForMember(dest => dest.CreatorId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CreatorFirstName, src => src.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.CreatorLastName, src => src.MapFrom(x => x.LastName));

            CreateMap<Question, UpdateQuestionCommand.Response>();
            CreateMap<Answer, UpdateQuestionCommand.AnswerDto>();
            CreateMap<UpdateQuestionCommand.Request, Question>();
            CreateMap<UpdateQuestionCommand.AnswerDto, Answer>();
            
        }
    }
}
