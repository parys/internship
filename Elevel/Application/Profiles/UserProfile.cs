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
            CreateMap<User, UsersDTO>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id));

            CreateMap<User, GetCoachesQuery.CoachDto>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id));

            CreateMap<User, GetUserByIdQuery.Response>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id));
        }
    }
}
