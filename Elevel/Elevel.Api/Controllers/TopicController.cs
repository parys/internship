using Elevel.Application.Features.TopicCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class TopicController : BaseApiController
    {
        public TopicController()
        {

        }

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

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetTopicByIdQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteTopicCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return response == null ? BadRequest() : Ok(response);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTopicCommand.Request request)
        {
            request.Id = id;
            return Ok(await Mediator.Send(request));
        }
    }
}
