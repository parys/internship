using Elevel.Application.Extensions;
using Elevel.Application.Features.UserFeatures;
using Elevel.Application.Interfaces;
using Elevel.Domain.Authentication;
using Elevel.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetUserListQuery.Request request)
        {
            var result = Mediator.Send(request);
            return Ok(await result);
        }

        /// <summary>
        /// Receives Email, and Password
        /// Returns Token, and Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }

        /// <summary>
        /// Receives piece of name parameter
        /// comparing with FirstName, LastName, and UserName 
        /// Returns list of coaches(id, FirstName, LastName, UserName, TestCount)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("coaches"), Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> GetCoachesAsync([FromQuery] GetCoachesQuery.Request request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

    }
}

