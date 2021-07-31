using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateTestAsync ([FromBody] CreateTestCommand.Request request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRole.HumanResourceManager)),HttpPost("assign")]
        public async Task<IActionResult> AssignTestAsync([FromBody] AssignTestCommand.Request request)
        {
            var claims = User.Claims.ToList();

            var userId = claims.FirstOrDefault(x => x.Type == "uid").Value;

            request.HrId = Guid.Parse(userId);

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpPut("startTest/{id:Guid}")]
        public async Task<IActionResult> StartTestByIdAsync([FromRoute] StartTestByIdQuery.Request request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTestsAsync([FromQuery] GetAllTestsQuery.Request request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpPut("submit")]
        public async Task<IActionResult> SubmitTestAsync([FromBody] SubmitTestCommand.Request request)
        {
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        [HttpPut("Check/{id:Guid}"), Authorize(Roles = nameof(UserRole.Coach))]
        public async Task<IActionResult> CheckTestAsync([FromRoute]Guid id, [FromBody] CheckTestCommand.Request request)
        {
            var claims = User.Claims.ToList();

            var userId = claims.FirstOrDefault(x => x.Type == "uid").Value;

            request.CoachId = Guid.Parse(userId);

            request.Id = id;

            var result = await Mediator.Send(request);

            return Ok(result);
        }
    }
}
