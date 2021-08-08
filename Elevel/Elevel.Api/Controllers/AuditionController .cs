using Elevel.Application.Features.AuditionCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class AuditionController : BaseApiController
    {
        public AuditionController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditionList([FromQuery] GetAuditionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAuditionById([FromRoute] GetAuditionDetailQuery.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateauditionAsync([FromBody] CreateAuditionCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAuditionAsync([FromRoute] Guid id, [FromBody] UpdateAuditionCommand.Request request)
        {
            if (request.Id != id)
            {
                return BadRequest("Id's from url and from body are different");
            }
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAuditionAsync([FromRoute] DeleteAuditionCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
