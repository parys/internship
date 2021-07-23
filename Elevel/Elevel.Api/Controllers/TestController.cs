using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elevel.Application.Features.TestQuestion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Elevel.Domain.Enums;
using Elevel.Application.Features.TestCommands;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : BaseApiController
    {

        //[HttpPut]//To update!!!!!!!!!!!!!
        //public async Task<IActionResult> SaveUserAnswerAsync([FromBody] UpdateTestQuestionCommand.Request request)
        //{
        //    await Mediator.Send(request);
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> CreateTestAsync ([FromBody] CreateTestCommand.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllTestsAsync([FromQuery] GetAllTestsQuery.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTestAsync([FromBody] UpdateTestCommand.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
