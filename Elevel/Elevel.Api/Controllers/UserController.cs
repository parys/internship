using Elevel.Application.Extensions;
using Elevel.Application.Features.ApplicationUserFeatures;
using Elevel.Application.Interfaces;
using Elevel.Domain.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
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

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetUserListQuery.Request request)
        {
            var result = Mediator.Send(request);
            return Ok(await result);
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetUsersByIdAsync([FromRoute] GetUserByIdQuery.Request request)
        {
            var result = Mediator.Send(request);
            return Ok(await result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }

        [HttpGet("info")]
        public async Task<IActionResult> UserInfo()
        {
            GetUserByIdQuery.Request request = new GetUserByIdQuery.Request()
            {
                Id = User.GetLoggedInUserId()
            };
            var result = await Mediator.Send(request);
            
            return Ok(result);
        }
    }
}

