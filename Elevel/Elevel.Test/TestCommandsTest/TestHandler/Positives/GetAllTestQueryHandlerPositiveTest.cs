using Elevel.Test.TestCommandsTest.TestHandler.Fixtures;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Handler = Elevel.Application.Features.TestCommands.GetAllTestsQuery.Handler;
using Request = Elevel.Application.Features.TestCommands.GetAllTestsQuery.Request;
using Response = Elevel.Application.Features.TestCommands.GetAllTestsQuery.Response;

namespace Elevel.Test.TestCommandsTest.TestHandler.Positives
{
    [Collection(nameof(TestEntityCollection))]
    public class GetAllTestQueryHandlerPositiveTest
    {
        private readonly IRequestHandler<Request, Response> _handler;

        public GetAllTestQueryHandlerPositiveTest(TestEntityFixture fixture)
        {
            _handler = new Handler(fixture.Context, fixture.Mapper);
        }

        [Fact]
        public async Task GetAllTestQueryHandlerPositiveTestExecution() {
            
            //Act

            var result = await _handler.Handle(new Request(), CancellationToken.None);
            
            //Assert

            result.Should().NotBeNull();
            result.Results.Count().Should().BeGreaterThan(0);
        }
    }
}
