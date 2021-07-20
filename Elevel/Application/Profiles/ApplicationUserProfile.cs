using AutoMapper;
using Elevel.Application.Features.ApplicationUserFeatures;
using Elevel.Domain.Models;
using static Elevel.Application.Features.ApplicationUserFeatures.GetAllApplicationUserQuery;

namespace Elevel.Application.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, GetApplicationUserByIdQuery.Response>();
            CreateMap<ApplicationUser, UsersDTO>();
        }
    }
}
