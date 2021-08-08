using AutoMapper;
using Elevel.Application.Features.AuditionCommands;
using Elevel.Domain.Models;
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
            CreateMap<Audition, CreateAuditionCommand.Response>();
            CreateMap<Audition, DeleteAuditionCommand.Response>();
            CreateMap<Audition, GetAuditionDetailQuery.Response>();
            CreateMap<Question, GetAuditionDetailQuery.QuestionDto>();
            CreateMap<Audition, GetAuditionListQuery.QuestionDto>();
            CreateMap<Audition, UpdateAuditionCommand.Response>();
            CreateMap<Question, UpdateAuditionCommand.QuestionDto>();
            CreateMap<UpdateAuditionCommand.Request, Audition>();
            CreateMap<UpdateAuditionCommand.QuestionDto, Question>();
        }
    }
}
