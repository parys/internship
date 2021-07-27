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
            CreateMap<Test, CreateTestCommand.Response>();

            CreateMap<Test, GetAllTestsQuery.TestDTO>();
            CreateMap<Test, UpdateTestCommand.Response>();
            CreateMap<UpdateTestCommand.Request, Test>();
           // CreateMap<Test, GetTestByIdQuery.Response>();
            CreateMap<AssignTestCommand.Request, Test>();
        }
    }
}
