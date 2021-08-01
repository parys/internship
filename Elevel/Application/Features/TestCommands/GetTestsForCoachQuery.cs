using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class GetTestsForCoachQuery
    {
        public class Request: IRequest<Response>
        {

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
            public string MyProperty { get; set; }
        }
    }
}
