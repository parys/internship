using Elevel.Application.Features.QuestionCommands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : BaseApiController
    {
        public QuestionController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionList(GetQuestionListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetQuestionById(GetQuestionDetailQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionAsync(CreateQuestionCommand.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateQuestionAsync(Guid id, UpdateQuestionCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteQuestionAsync(Guid id, DeleteQuestionCommand.Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(request));
        }
    }
}