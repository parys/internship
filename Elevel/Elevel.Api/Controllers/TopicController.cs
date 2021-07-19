using Elevel.Application.Features.TopicCommands;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMediator _mediator;
        public TopicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize,HttpGet]
        public async Task<IActionResult> GetTopicList([FromBody] GetTopicListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [Authorize, HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetTopicById([FromBody] GetTopicDetailQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> CreateTopicAsync([FromBody] CreateTopicCommand.Request request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [Authorize, HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateTopicAsync([FromRoute] Guid id, [FromBody] UpdateTopicCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [Authorize, HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteTopicAsync([FromRoute] Guid id, [FromBody] DeleteTopicCommand.Request request)
        {
            if(id != request.Id)
            {
                return BadRequest();
            }
            var res = await _mediator.Send(request);
            return Ok(res);
        }
    }
}
