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
        public TopicController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateTopicCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( GetTopicListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("id:Guid")]
        public async Task<IActionResult> GetById( GetTopicByIdQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id, DeleteTopicCommand.Request request)
        {
            if(id != request.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTopicCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(request));
        }
    }
}
