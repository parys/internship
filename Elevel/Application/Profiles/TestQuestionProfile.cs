using AutoMapper;
using Elevel.Application.Features.TestQuestion;
using Elevel.Domain.Models;

namespace Elevel.Application.Profiles
{
    public class TestQuestionProfile : Profile
    {
        public TestQuestionProfile()
        {
            CreateMap<UpdateTestQuestionCommand.Request, TestQuestion>();
        }
    }
}