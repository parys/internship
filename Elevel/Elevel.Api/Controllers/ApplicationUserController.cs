﻿using Elevel.Application.Features.ApplicationUserFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Authorize(Roles = "HumanResourceManager")]
    [Route("api/[controller]")]
    public class ApplicationUserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ApplicationUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize, HttpGet("")]
        public  async Task<IActionResult> GetAllUsersAsync([FromQuery] GetAllApplicationUserQuery.Request request )
        {
            var result = _mediator.Send(request);
            return Ok(await result);
        }

        [Authorize, HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetUsersByIdAsync([FromRoute] GetApplicationUserByIdQuery.Request request)
        {
            var result = _mediator.Send(request);
            return Ok(await result);
        }

    }
}
