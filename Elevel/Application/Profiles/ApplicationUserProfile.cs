using AutoMapper;
using Elevel.Application.Features.ApplicationUserFeatures;
using Elevel.Domain.Models;

namespace Elevel.Application.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, GetApplicationUserByIdQuery.Response>();
        }
    }
}
