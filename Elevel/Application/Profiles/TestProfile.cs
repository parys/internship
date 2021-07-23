using AutoMapper;
using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Models;

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
