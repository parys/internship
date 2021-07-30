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
            var claims = User.Claims.ToList();
            var userId = claims.FirstOrDefault(x=>x.Type == "uid").Value;
            var exp = claims.FirstOrDefault(x => x.Type == "exp").Value;

            GetApplicationUserByIdQuery.Request request = new GetApplicationUserByIdQuery.Request()
            {
                Id = Guid.Parse(userId),
            };
            var result = await Mediator.Send(request);
            
            return Ok(result);
        }
    }
}

