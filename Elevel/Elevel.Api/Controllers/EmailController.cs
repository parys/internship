using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : BaseApiController
    {
        private readonly IMailService _mailService;
        public EmailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("Send")]
        public async Task <IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
