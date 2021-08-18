using Elevel.Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Elevel.Application.Features.ReportCommands;
using Elevel.Domain.Enums;

namespace Elevel.Api.Controllers
{
    [Authorize, Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        /// <summary>
        /// Get list with reports.
        /// Receive ... from query.
        /// Returns all reports.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetReportList([FromQuery] GetReportListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Get report by Id.
        /// Receives id from route.
        /// Returns Id, ...
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetReportById([FromRoute] GetReportDetailQuery.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Create report.
        /// Receives ...
        /// Return Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateReportAsync([FromBody] CreateReportCommand.Request request)
        {
            request.UserId = User.GetLoggedInUserId();
            
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Update report.
        /// Receives Id from route, Receives ... from body
        /// Return Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> UpdateReportAsync([FromRoute] Guid id,
            [FromBody] UpdateReportCommand.Request request)
        {
            if (request.Id != id)
            {
                return BadRequest("Id's from url and from body are different");
            }

            var response = await Mediator.Send(request);

            return Ok(response);
        }
    }
}
