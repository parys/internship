using Elevel.Application.Features.TopicCommands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTopicCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetTopicListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("id:Guid")]
        public async Task<IActionResult> GetById([FromBody] GetTopicByIdQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteTopicCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTopicCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(request));
        }
    }
}
