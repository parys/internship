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

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
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

                if(request.SpeakingMark < Constants.MIN_MARK 
                    || request.SpeakingMark > Constants.MAX_MARK )
                {
                    throw new ValidationException("Speaking mark number is out if range from 0 to 10");
                }

                if(request.EssayMark < Constants.MIN_MARK
                    || request.EssayMark > Constants.MAX_MARK)
                {
                    throw new ValidationException("Essay mark number is out if range from 0 to 10");
                }

                

                test.SpeakingMark = request.SpeakingMark;

                test.EssayMark = request.EssayMark;

                test.Comment = request.Comment;

                await _context.SaveChangesAsync();


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
