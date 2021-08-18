using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class AssignTestCommand
    {
        public class Request : IRequest<Response>
        {
            public DateTimeOffset AssignmentEndDate { get; set; }

            public Guid UserId { get; set; }

            public bool Priority { get; set; } = false;

            [JsonIgnore]
            public Guid HrId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.AssignmentEndDate).NotEmpty().WithMessage("AssignmentEndDate can't be empty or null!");

                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId can't be empty or null!");

            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            private static Random _rand = new Random();

            private readonly UserManager<User> _userManager;

            private readonly IMailService _mailService;

            public Handler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager, IMailService mailService)
            {
                _context = context;

                _mapper = mapper;

                _userManager = userManager;

                _mailService = mailService;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (!await _userManager.Users.AnyAsync(x => x.Id == request.UserId))
                {
                    throw new NotFoundException($"User with {request.UserId}");
                }

                if (request.HrId == request.UserId)
                {
                    throw new ValidationException("You can't assign test to yourself");
                }

                if (request.AssignmentEndDate.Date < DateTimeOffset.UtcNow.Date)
                {
                    throw new ValidationException($"assignmentEndDate can't be in the past ({request.AssignmentEndDate})");
                }

                //if (await _context.Tests.AnyAsync(x => x.UserId == request.UserId
                // && ((DateTimeOffset)x.AssignmentEndDate).Date <= DateTimeOffset.UtcNow.Date
                // && !x.AuditionMark.HasValue))
                //{
                //    throw new ValidationException($"User with id {request.UserId} hasn't passed the previous assigned test for today");
                //}

                var test = _mapper.Map<Test>(request);

                test.Id = Guid.NewGuid();

                await _context.Tests.AddAsync(test);

                await _context.SaveChangesAsync(cancellationToken);

                _mailService.SendMessage(request.UserId,
                    "You was assigned to the test",
                    "<center><table width=\"500px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style=\"text-align: center; font-size: large;\">"
                    + "You was assigned to complete the test by Elevel's HR.<br/>"
                    + "Please go to the following link to enter the Elevel site: <br/>"
                    + "<a href=\"http://exadel-train-app.herokuapp.com\">Enter the Elevel site</a><br/><br/>"
                    + "<img src =\"https://habrastorage.org/getpro/moikrug/uploads/company/663/850/800/logo/medium_57e7286b71f5bb50f7eda0c9bce2cf99.png\" alt =\"Exadel Logo\" style=\"margin: auto; display:inline; width: 200px;\">"
                    + "</td></tr></table></center>");
                return new Response { Id = test.Id };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}