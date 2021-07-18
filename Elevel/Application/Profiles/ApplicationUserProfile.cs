using AutoMapper;
using Elevel.Domain.Models;
using static Elevel.Application.Features.ApplicationUserFeatures.GetAllApplicationUserQuery;

namespace Elevel.Application.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
