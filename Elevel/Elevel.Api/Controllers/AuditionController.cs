using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Elevel.Application.Features.AuditionFeatures;
using MediatR;

namespace Elevel.Api.Controllers
{

    [Authorize, Route("[controller]/[action]")]
    public class AuditionController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> CreateAuditionAsync([FromBody] CreateAuditionCommand.Request request)
        {
            var res = await Mediator.Send(request);
            return Ok(res);
        }
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAuditionAsync([FromRoute] Guid id, [FromBody] UpdateAuditionCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var res = await Mediator.Send(request);
            return Ok(res);
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAuditionAsync([FromRoute] Guid id, [FromBody] DeleteAuditionCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var res = await Mediator.Send(request);
            return Ok(res);
        }
        [HttpGet]
        public async Task<IActionResult> GetAuditionList([FromBody] GetAuditionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAuditionById([FromBody] GetAuditionByIdQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}

