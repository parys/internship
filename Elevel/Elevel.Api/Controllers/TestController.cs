using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : BaseApiController
    {

        //[HttpPut]//To update!!!!!!!!!!!!!
        //public async Task<IActionResult> SaveUserAnswerAsync([FromBody] UpdateTestQuestionCommand.Request request)
        //{
        //    await Mediator.Send(request);
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> CreateTestAsync ([FromBody] CreateTestCommand.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [Authorize(Roles = nameof(UserRole.HumanResourceManager)),HttpPost("assign")]
        public async Task<IActionResult> AssignTestAsync([FromBody] AssignTestCommand.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("startTest/{id:Guid}")]
        public async Task<IActionResult> GetTestByIdAsync([FromRoute] StartTestByIdQuery.Request request)
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
        public async Task<IActionResult> UpdateTestAsync([FromBody] SubmitTestCommand.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
