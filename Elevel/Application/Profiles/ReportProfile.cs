using System;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Elevel.Application.Features.AuditionCommands;
using Elevel.Application.Features.ReportCommands;
using Elevel.Domain.Models;

namespace Elevel.Application.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<CreateReportCommand.Request, Report>();
            CreateMap<Report, GetReportListQuery.ReportDto>()
                .ForMember(dest => dest.CoachName,
                    opt => opt.MapFrom(src =>
                        src.Question.Creator.UserName ?? src.Audition.Creator.UserName ?? src.Topic.Creator.UserName))
                .ForMember(x => x.CoachId,
                    opt => opt.MapFrom(src =>
                        src.Question != null ? src.Question.CreatorId :
                        src.Audition != null ? src.Audition.CreatorId :
                        src.Topic != null ? src.Topic.CreatorId : Guid.Empty));

            CreateMap<Report, GetReportDetailQuery.Response>()
                .ForMember(dest => dest.CoachName,
                    opt => opt.MapFrom(src =>
                        src.Creator.UserName))
                .ForMember(dest => dest.CoachId,
                    opt => opt.MapFrom(src =>
                        src.Creator.Id))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src =>
                        src.User.UserName))
                .ForMember(dest => dest.UserId,
                    opt => opt.MapFrom(src =>
                        src.User.Id));

            CreateMap<UpdateReportCommand.Request, Report>();
        }
    }
}