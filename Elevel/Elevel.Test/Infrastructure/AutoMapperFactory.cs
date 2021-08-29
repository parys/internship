using AutoMapper;
using Elevel.Application.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Test.Infrastructure
{
    public static class AutoMapperFactory
    {
        public static IMapper Create()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddMaps(typeof(TestProfile),
                    typeof(QuestionProfile),
                    typeof(AuditionProfile),
                    typeof(ReportProfile),
                    typeof(TestQuestionProfile),
                    typeof(TopicProfile),
                    typeof(UserProfile));
            });

            return mappingConfig.CreateMapper();
        }
    }
}
