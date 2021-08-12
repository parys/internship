using Elevel.Application.Features.AuditionCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Elevel.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace Elevel.Api.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class AuditionController : BaseApiController
    {
        public AuditionController()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        public async Task<IActionResult> CreateAuditionAsync([FromBody] CreateAuditionCommand.Request request)
        {
            request.CreatorId = User.GetLoggedInUserId();
            foreach (var question in request.Questions)
            {
                question.CreatorId = User.GetLoggedInUserId();
                question.Level = request.Level;
            }
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
