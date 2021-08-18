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
            CreateMap<Report, GetReportListQuery.ReportDto>();

            CreateMap<Report, GetReportDetailQuery.Response>();
            CreateMap<User, GetReportDetailQuery.UserDTO>();
            CreateMap<User, GetReportDetailQuery.CoachDTO>();

            CreateMap<UpdateReportCommand.Request, Report>();
        }
    }
}