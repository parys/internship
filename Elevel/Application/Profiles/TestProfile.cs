using AutoMapper;
using Elevel.Application.Features.ApplicationUserFeatures;
using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Models;
using static Elevel.Application.Features.ApplicationUserFeatures.GetAllApplicationUserQuery;

namespace Elevel.Application.Profiles
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<CreateTestCommand.Request, Test>();
            CreateMap<Test, GetAllTestsQuery.TestDTO>();
            CreateMap<Test, UpdateTestCommand.Response>();
            CreateMap<UpdateTestCommand.Request, Test>();
        }
    }
}
