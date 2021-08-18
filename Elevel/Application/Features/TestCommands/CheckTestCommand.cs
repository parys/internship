using AutoMapper;
using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class CheckTestCommand
    {
        public class Request : IRequest<Response>
        {

            public int SpeakingMark { get; set; }

            public int EssayMark { get; set; }

            public string Comment { get; set; }

            [JsonIgnore]
            public Guid Id { get; set; }

            [JsonIgnore]
            public Guid CoachId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.SpeakingMark)
                    .InclusiveBetween(Constants.MIN_MARK,Constants.MAX_MARK)
                    .WithMessage("Speaking mark number is out if range from 0 to 10!");

                RuleFor(x => x.EssayMark)
                    .InclusiveBetween(Constants.MIN_MARK, Constants.MAX_MARK)
                    .WithMessage("Essay mark number is out if range from 0 to 10!");
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IMailService _mailService;

            public Handler(IApplicationDbContext context, IMapper mapper, IMailService mailService)
            {
                _context = context;
                _mapper = mapper;
                _mailService = mailService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var test = await _context.Tests.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (test == null)
                {
                    throw new NotFoundException($"Test with Id {request.Id}");
                }

                if (test.CoachId != request.CoachId 
                    || test.UserId == request.CoachId)
                {
                    throw new ValidationException("You can't check this test");
                }

                test.SpeakingMark = request.SpeakingMark;

                test.EssayMark = request.EssayMark;

                test.Comment = request.Comment;

                await _context.SaveChangesAsync();

                if (test.HrId.HasValue)
                {
                    _mailService.SendMessage((Guid)test.HrId,
                         "The test you assigned to user was checked",
                         "<center><table width=\"500px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style=\"text-align: center; font-size: large;\">"
                         + "The test which was assigned to user {FirstName} {LastName} ({Email} by you is checked now.<br/>"
                         + "Please go to the following link to see the marks: <br/>"
                         + "<a href=\"http://exadel-train-app.herokuapp.com/home\">Enter the Elevel site</a><br/><br/>"
                         + "<img src=\"https://habrastorage.org/getpro/moikrug/uploads/company/663/850/800/logo/medium_57e7286b71f5bb50f7eda0c9bce2cf99.png\" alt =\"Exadel Logo\" style=\"margin: auto; display:inline; width: 200px;\">"
                         +"</td></tr></table></center>");
                }

                _mailService.SendMessage(test.UserId,
                    "Your test was checked",
                    "<center><table width=\"500px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style=\"text-align: center; font-size: large;\">"
                    + "The test which was assigned to you is checked now.<br/>"
                    + "Please go to the following link to see the marks: <br/>"
                    + "<a href=\"http://exadel-train-app.herokuapp.com/home\">Enter the Elevel site</a><br/><br/>"
                    + "<img src=\"https://habrastorage.org/getpro/moikrug/uploads/company/663/850/800/logo/medium_57e7286b71f5bb50f7eda0c9bce2cf99.png\" alt =\"Exadel Logo\" style=\"margin: auto; display:inline; width: 200px;\"><br/>"
                    + "</td></tr></table></center>");

                return _mapper.Map<Response>(test);
            }
        }
        public class Response
        {
            public Level Level { get; set; }

            public long TestNumber { get; set; }

            public int? EssayMark { get; set; }

            public int? SpeakingMark { get; set; }

            public string Comment { get; set; }

            public Guid? CoachId { get; set; }
        }
    }
}
