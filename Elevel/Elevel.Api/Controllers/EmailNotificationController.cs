using Elevel.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailNotificationController : ControllerBase
    {

        [HttpPost]
        public async Task <IActionResult> SendEmail()
        {
            EmailNotification emailNotification = new EmailNotification();
            await emailNotification.SendEmailAsync("evgenijj1982@gmail.com", "Body", "Test");
            return RedirectToAction("Index");
        }
    }
}
