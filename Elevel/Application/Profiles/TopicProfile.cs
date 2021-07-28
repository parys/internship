using AutoMapper;
using Elevel.Application.Features.TopicCommands;
using Elevel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Profiles
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<CreateTopicCommand.Request, Topic>();
            CreateMap<Topic, DeleteTopicCommand.Response>();
            CreateMap<Topic, GetTopicByIdQuery.Response>();
            CreateMap<Topic, GetTopicListQuery.TopicListDto>();
            CreateMap<Topic, UpdateTopicCommand.Response>();
            CreateMap<UpdateTopicCommand.Request, Topic>();
        }
    }
}
