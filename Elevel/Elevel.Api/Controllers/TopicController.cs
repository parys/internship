using Elevel.Application.Extensions;
using Elevel.Application.Features.TopicCommands;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize(Roles = nameof(UserRole.Coach)), Route("api/[controller]")]
    public class TopicController : BaseApiController
    {
        public TopicController()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTopicCommand.Request request)
        {
            request.CreatorId = User.GetLoggedInUserId();

            return Ok(await Mediator.Send(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetTopicListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetTopicByIdQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] DeleteTopicCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTopicCommand.Request request)
        {
            if(id != request.Id)
            {
                return BadRequest("Ids from url and from body are different");
            }

            request.CreatorId = User.GetLoggedInUserId();

            return Ok(await Mediator.Send(request));
        }
    }
}
