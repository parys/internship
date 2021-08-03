using AutoMapper;
using Elevel.Application.Features.ApplicationUserFeatures;
using Elevel.Domain.Models;
using static Elevel.Application.Features.ApplicationUserFeatures.GetUserListQuery;

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
