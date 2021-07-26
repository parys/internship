using AutoMapper;
using Elevel.Application.Features.AuditionFeatures;
using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elevel.Application.Features.AuditionFeatures.GetAuditionListQuery;

namespace Elevel.Application.Profiles
{
    public class AuditionProfile: Profile
    {
        public AuditionProfile()
        {
            CreateMap<Audition, GetAuditionByIdQuery.Response>();
            CreateMap<Audition, AuditionDto>();
            CreateMap<UpdateAuditionCommand.Request, Audition>();
        }
    }
}
