using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class AssignTestForCoachCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid TestId { get; set; }
            public Guid CoachId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IMailService _mailService;

            public Handler(IApplicationDbContext context, UserManager<User> userManager, IMailService mailService)
            {
                _userManager = userManager;
                _context = context;
                _mailService = mailService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var test = await _context.Tests.FirstOrDefaultAsync(x => x.Id == request.TestId);

                if (test is null)
                {
                    throw new NotFoundException($"Test with id {request.TestId}");
                }

                if (test.UserId == request.CoachId)
                {
                    throw new ValidationException("You can't assign this test to this coach!");
                }

                if (test.SpeakingMark.HasValue && test.EssayMark.HasValue)
                {
                    throw new ValidationException("This test has already checked!");
                }

                var coach = await _userManager.FindByIdAsync(request.CoachId.ToString());

                if (coach is null || !(await _userManager.GetRolesAsync(coach)).Contains(UserRole.Coach.ToString()))
                {
                    throw new ValidationException($"User is either not found or not coach");
                }

                test.CoachId = request.CoachId;

                await _context.SaveChangesAsync(cancellationToken);

                _mailService.SendMessage(request.CoachId,
                    "You was assigned to the test",
                    "<center><table width=\"500px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style=\"text-align: center; font-size: large;\">"
                    + "You was assigned to complete the test by Elevel's HR.<br/>"
                    + "Please go to the following link to enter the Elevel site: <br/>"
                    + "<a href=\"http://exadel-train-app.herokuapp.com\">Enter the Elevel site</a><br/><br/>"
                    + "<img src=\"https://habrastorage.org/getpro/moikrug/uploads/company/663/850/800/logo/medium_57e7286b71f5bb50f7eda0c9bce2cf99.png\" alt =\"Exadel Logo\" style=\"margin: auto; display:inline; width: 200px;\">"
                    + "</td></tr></table></center>");

                return new Response
                {
                    TestId = test.Id,
                    CoachId = (Guid)test.CoachId
                };
            }
        }

        public class Response
        {
            public Guid TestId { get; set; }
            public Guid CoachId { get; set; }
        }
    }
}
