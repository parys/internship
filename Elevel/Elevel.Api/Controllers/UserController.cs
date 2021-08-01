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

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }

        [HttpGet("info")]
        public async Task<IActionResult> UserInfo()
        {
            GetApplicationUserByIdQuery.Request request = new GetApplicationUserByIdQuery.Request()
            {
                Id = User.GetLoggedInUserId()
            };
            var result = await Mediator.Send(request);
            
            return Ok(result);
        }
    }
}

