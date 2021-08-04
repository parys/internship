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
            CreateMap<Question, CreateTestCommand.QuestionDto>();
            CreateMap<Answer, CreateTestCommand.AnswerDto>();
            CreateMap<Topic, CreateTestCommand.TopicDto>();
            CreateMap<Audition, CreateTestCommand.AuditionDto>();

            CreateMap<Test, GetAllTestsQuery.TestDTO>();

            CreateMap<Test, SubmitTestCommand.Response>();
            CreateMap<SubmitTestCommand.Request, Test>();

            CreateMap<Test, StartTestByIdQuery.Response>();
            CreateMap<Question, StartTestByIdQuery.QuestionDto>();
            CreateMap<Answer, StartTestByIdQuery.AnswerDto>();
            CreateMap<Topic, StartTestByIdQuery.TopicDto>();
            CreateMap<Audition, StartTestByIdQuery.AuditionDto>();

            CreateMap<AssignTestCommand.Request, Test>();

            CreateMap<Test, CheckTestCommand.Response>();

            CreateMap<Test, GetTestsForCoachQuery.TestDto>();

        }
    }
}
