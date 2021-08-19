using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

                var testForCheckAmount = _context.Tests.Where(x => !x.EssayMark.HasValue && x.CoachId == test.Coach.Id).Count();

                _mailService.NotifyUser(test.Coach.Email,
                    "You were assigned to check the test",
                    "You were assigned to evaluate the test by Elevel's Admin.<br/>"
                    + $"{(testForCheckAmount == 1 ? $"{testForCheckAmount} test is" : $"{testForCheckAmount} tests are")} waiting for your check<br/>"
                    + "Please go to the following link to enter the Elevel site: <br/>"
                    + "<a href=\"http://exadel-train-app.herokuapp.com\">Enter the Elevel site</a><br/><br/>");

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
