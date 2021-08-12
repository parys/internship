using Elevel.Application.Features.AuditionCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Elevel.Application.Extensions;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Elevel.Api.Controllers
{
    [Authorize(Roles = nameof(UserRole.Coach)), Route("api/[controller]")]
    public class AuditionController : BaseApiController
    {
        public AuditionController()
        {
        }

        /// <summary>
        /// Get list with auditions.
        /// Receive AuditionNumber and Level from query.
        /// Returns all not deleted auditions.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAuditionList([FromQuery] GetAuditionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Get audition by Id.
        /// Receives id from route.
        /// Returns Id, AuditionNumber, AudioFilePath, Level, CreationDate, CreatorId,
        /// List of Questions (Id, NameQuestion, Level,
        /// List of answers (Id, NameAnswer, IsRight))
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAuditionById([FromRoute] GetAuditionDetailQuery.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Create audition.
        /// Receives AudioFilePath, Level,
        /// List of Questions (Id, NameQuestion, Level,
        /// List of answers (Id, NameAnswer, IsRight)) from body.
        /// Return Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update audition.
        /// Receives Id from route, Receives AudioFilePath, Level,
        /// List of Questions (Id, NameQuestion,
        /// List of answers (Id, NameAnswer, IsRight)) from body
        /// Return Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete audition.
        /// Receive Id from route.
        /// Return Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAuditionAsync([FromRoute] DeleteAuditionCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
