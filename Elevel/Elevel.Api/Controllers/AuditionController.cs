using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Elevel.Application.Features.AudioFeatures;
using MediatR;

namespace Elevel.Api.Controllers
{
    public class AuditionController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AuditionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize, HttpPost]
        public async Task<IActionResult> CreateAuditionAsync([FromBody] CreateAuditionCommand.Request request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }
        [Authorize, HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAuditionAsync([FromRoute] Guid id, [FromBody] UpdateAuditionCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var res = await _mediator.Send(request);
            return Ok(res);
        }
        [Authorize,HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuditionAsync([FromRoute] Guid id, [FromBody] DeleteAudiotionCommand.Request request)
        {
            if(id != request.Id)
            {
                return BadRequest();
            }
            var res = await _mediator.Send(request);
            return Ok(res);
        }
        [Authorize,HttpGet]
        public async Task<IActionResult> GetAuditionList([FromBody] GetAuditionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
