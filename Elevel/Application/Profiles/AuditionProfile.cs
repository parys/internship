using AutoMapper;
using Elevel.Application.Features.AuditionCommands;
using Elevel.Domain.Models;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Profiles
{
    public class AuditionProfile : Profile
    {
        public AuditionProfile()
        {
            CreateMap<CreateAuditionCommand.Request, Audition>();
            CreateMap<CreateAuditionCommand.QuestionDto, Question>();
            CreateMap<CreateAuditionCommand.AnswerDto, Answer>();

            CreateMap<Audition, GetAuditionDetailQuery.Response>();
            CreateMap<Answer, GetAuditionDetailQuery.AnswerDto>();
            CreateMap<Question, GetAuditionDetailQuery.QuestionDto>();

            CreateMap<Audition, GetAuditionListQuery.AuditionDto>();
            CreateMap<User, GetAuditionListQuery.AuditionDto>()
                .ForMember(dest => dest.CreatorId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CreatorFirstName, src => src.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.CreatorLastName, src => src.MapFrom(x => x.LastName));

            CreateMap<Audition, UpdateAuditionCommand.Response>();
            CreateMap<Question, UpdateAuditionCommand.QuestionDto>();
            CreateMap<Answer, UpdateAuditionCommand.AnswerDto>();
            CreateMap<UpdateAuditionCommand.AnswerDto, Answer>()
                .EqualityComparison((dto, model) => dto.Id == model.Id);
            CreateMap<UpdateAuditionCommand.Request, Audition>();
            CreateMap<UpdateAuditionCommand.QuestionDto, Question>()
                .EqualityComparison((dto, model) => dto.Id == model.Id);
        }
    }
}
