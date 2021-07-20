using AutoMapper;
using Elevel.Domain.Models;

namespace Elevel.Application.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, Features.ApplicationUserFeatures.GetAllApplicationUserQuery.UserDto>();
        }
    }
}
