using Elevel.Application.Extensions;
using Elevel.Application.Features.QuestionCommands;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize(Roles = nameof(UserRole.Coach)), Route("api/[controller]")]
    public class QuestionController : BaseApiController
    {
        /// <summary>
        /// Receive CreatorId, QuestionNumber, and Level from query.
        /// Returns all not deleted questions without answers.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetQuestionList([FromQuery]GetQuestionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Receives id from route.
        /// Returns Question Id, Question Nuber, Level, Question Text, List of Answers(Id, QuestionId, Answer text, IsRight)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetQuestionById([FromRoute]GetQuestionDetailQuery.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Receives Level, Question Text, List of Answers(Answer text, IsRight) from body
        /// Returns test id 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateQuestionAsync([FromBody] CreateQuestionCommand.Request request)
        {
            request.CreatorId = User.GetLoggedInUserId();
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Receives id from route; Level, Question Text, List of Answers(Answer text, IsRight) from body
        /// Returns test id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateQuestionAsync([FromRoute] Guid id, [FromBody] UpdateQuestionCommand.Request request)
        {
            request.Id = id;

            var response = await Mediator.Send(request);

            return Ok(response);
        }

        /// <summary>
        /// Receives id from route
        /// Returns id and Deleted flag
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteQuestionAsync([FromRoute] DeleteQuestionCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}