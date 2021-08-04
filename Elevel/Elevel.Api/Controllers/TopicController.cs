using Elevel.Application.Features.TopicCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] UpdateTopicCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
