using Elevel.Application.Features.ApplicationUserFeatures;
using Elevel.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize(Roles = nameof(UserRole.HumanResourceManager))]
    [Route("api/[controller]")]
    public class ApplicationUserController : BaseApiController
    {
        [Authorize, HttpGet("")]
        public  async Task<IActionResult> GetAllUsersAsync([FromQuery] GetAllApplicationUserQuery.Request request )
        {
            var result = Mediator.Send(request);
            return Ok(await result);
        }

        [Authorize, HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetUsersByIdAsync([FromRoute] GetApplicationUserByIdQuery.Request request)
        {
            var result = Mediator.Send(request);
            return Ok(await result);
        }
    }
}
