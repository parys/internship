using Elevel.Application.Features.TestCommands;
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
        
        [HttpGet]
        public async Task<IActionResult> GetAllTestsAsync([FromQuery] GetAllTestsQuery.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTestAsync([FromBody] UpdateTestCommand.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
