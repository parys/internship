using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elevel.Application.Features.Question;
using Elevel.Domain.Models;
using MediatR;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("single-file")]
        public async Task<ActionResult> UploadFileAsync(IFormFile file)
        {
            MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var request = new ImportQuestionsCommand.Request
            {
                InputFileStream = memoryStream
            };

            await _mediator.Send(request);
            return Ok();
        }
    }
}
