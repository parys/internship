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
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        /// <summary>
        /// Get list with reports.
        /// Returns all reports with status "Created".
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Coach))]
        [HttpGet]
        public async Task<IActionResult> GetReportList([FromQuery] GetReportListQuery.Request request)
        {
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Get report by Id.
        /// Receives id from route.
        /// Returns Id, Description, CreationDate, ReportStatus,
        /// User (UserName)
        /// Coach (CoachName)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Coach))]
        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetReportById([FromRoute] GetReportDetailQuery.Request request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Create report.
        /// Receives TestId, Description
        /// If report in Grammar, add only QuestionsId
        /// If report in Listening, add AuditionId and QuestionId
        /// If report in Writing or Speaking, add TopicId
        /// Return Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReportAsync([FromBody] CreateReportCommand.Request request)
        {
            request.UserId = User.GetLoggedInUserId();
            
            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        ///NOT FINISH
        /// 
        /// Update report.
        /// Receives Id from route, Receives ... from body
        /// Return Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Coach))]
        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> UpdateReportAsync([FromRoute] Guid id,
            [FromBody] UpdateReportCommand.Request request)
        {
            if (request.Id != id)
            {
                return BadRequest("Id's from url and from body are different");
            }

            request.CoachId = User.GetLoggedInUserId();

            var response = await Mediator.Send(request);

            return Ok(response);
        }
    }
}
