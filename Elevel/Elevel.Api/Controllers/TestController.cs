using Elevel.Application.Extensions;
using Elevel.Application.Features.TestCommands;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : BaseApiController
    {

        /// <summary>
        /// For "Start test" button without assignment. 
        /// Receives test level form body, UserId from token.
        /// Returns the whole test
        /// </summary>
        /// <param name="request">UserId (from token), Level</param>
        /// <returns>
        /// The whole test information 
        /// Such as: 
        ///  * Id
        ///  * TestNumber
        ///  * Level
        ///  * UserId
        ///  * TestPassingDate
        ///  * Audition with List of Questions with List of Answers
        ///  * Essay
        ///  * Speaking
        ///  * List of GrammarQuestions with List of Answers
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateTestAsync([FromBody] CreateTestCommand.Request request)
        {
            request.UserId = User.GetLoggedInUserId();
            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Can only be done by HR from "Assign test". 
        /// Receives AssignmentEndDate, UserId, Priority from body, and HrId from token. 
        /// Returns test id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("assign"), Authorize(Roles = nameof(UserRole.HumanResourceManager))]
        public async Task<IActionResult> AssignTestAsync([FromBody] AssignTestCommand.Request request)
        {
            request.HrId = User.GetLoggedInUserId();

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Can only be done dy assigned for this test user. 
        /// Receives level from body, UserId from token, and TestId from route
        /// Returns the whole test
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}/start")]
        public async Task<IActionResult> StartTestByIdAsync([FromRoute] Guid id, [FromBody] StartTestByIdQuery.Request request)
        {
            request.Id = id;

            request.UserId = User.GetLoggedInUserId();

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Get all test test. 
        /// May receive Id, UserId, TestPassingDate, and Level from query
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTestsAsync([FromQuery] GetAllTestsQuery.Request request)
        {
            var userRole = User.GetLoggedInUserRole();
            if (userRole != nameof(UserRole.HumanResourceManager)
                || User.GetLoggedInUserId() != request.UserId)
            {
                return Forbid();
            }

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Can be only done by assigned for this user. 
        /// Receives Id from route, UserId from token, List of GrammarAnswers, List of AuditionAnswers, EssayAnswer, and SpeakingAnswerReference from body. 
        /// Returns Id, Level, TestPassingDate, GrammarMark, AuditionMark, and UserId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}/submit")]
        public async Task<IActionResult> SubmitTestAsync([FromRoute] Guid id, [FromBody] SubmitTestCommand.Request request)
        {
            request.Id = id;

            request.UserId = User.GetLoggedInUserId();

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Can only be done by Coach.
        /// Receives TestId from route, coachId from token, SpeakingMark, EssayMark, and Comment from body.
        /// Returns Level, TestNumber, EssayMark, SpeakingMark, Comment, CoachId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}/check"), Authorize(Roles = nameof(UserRole.Coach))]
        public async Task<IActionResult> CheckTestAsync([FromRoute] Guid id, [FromBody] CheckTestCommand.Request request)
        {
            request.CoachId = User.GetLoggedInUserId();

            request.Id = id;

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Receives: IsChecked(false default), TestPassingDate, Level, Priority from query.
        /// Returns: Id, TestNumber, Level, TestPassingDate, Priority, EssayAnswer, SpeakingAnswerReference
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("forCoach"), Authorize(Roles = nameof(UserRole.Coach))]
        public async Task<IActionResult> GetAllTestForCoach([FromQuery] GetTestsForCoachQuery.Request request)
        {
            request.CoachId = User.GetLoggedInUserId();

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Receives: IsAssigned(false default), TestPassingDate, Level, Priority from query.
        /// Returns: Id, TestNumber, Level, TestPassingDate, Priority, coachId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("forAdmin"), Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> GetAllTestForAdmin([FromQuery] GetTestsForAdminQuery.Request request)
        {

            var result = await Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Receives TestId in route and body and CoachId from body.
        /// Returns TestId and CoachId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}/assignForCoach"), Authorize(Roles =nameof(UserRole.Administrator))]
        public async Task<IActionResult> AssignTestForCoach([FromRoute] Guid id, [FromBody] AssignTestForCoachCommand.Request request)
        {
            if(id != request.TestId)
            {
                return BadRequest("Route and Body ids don't match!");
            }
            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
