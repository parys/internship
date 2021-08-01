using AutoMapper;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class GetTestsForCoachQuery
    {
        public class Request: IRequest<Response>
        {
            [JsonIgnore]
            public Guid CoachId { get; set; }

            public bool? IsChecked { get; set; }

            public DateTimeOffset? MyProperty { get; set; }

            public Level? Level{ get; set; }

            public bool? Priority { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;

                _mapper = mapper;
            }

            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
        public class Response
        {
            public List<TestDto> Tests { get; set; }
        }
        public class TestDto
        {
            public Guid Id { get; set; }
            public long TestNumber { get; set; }
            public string EssayAnswer { get; set; }
            public string SpeakingAnswerReference { get; set; }
        }
    }
}
