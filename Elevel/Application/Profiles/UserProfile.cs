using AutoMapper;
using Elevel.Application.Features.UserFeatures;
using Elevel.Domain.Models;
using static Elevel.Application.Features.UserFeatures.GetUserListQuery;

namespace Elevel.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserByIdQuery.Response>();
            CreateMap<User, UsersDTO>();
        }
    }
}
