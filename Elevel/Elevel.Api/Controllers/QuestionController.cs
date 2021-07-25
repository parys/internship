using Elevel.Application.Features.QuestionCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class QuestionController : BaseApiController
    {
        public QuestionController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionList([FromQuery]GetQuestionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetQuestionById([FromRoute]GetQuestionDetailQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionAsync([FromBody] CreateQuestionCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return response == null ? BadRequest() : Ok(response);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateQuestionAsync([FromBody] UpdateQuestionCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return response == null ? BadRequest() : Ok(response);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteQuestionAsync([FromRoute] DeleteQuestionCommand.Request request)
        {
            var response = await Mediator.Send(request);
            return response == null ? BadRequest() : Ok(response);
        }
    }
}