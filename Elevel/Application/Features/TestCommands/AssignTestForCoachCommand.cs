﻿using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.TestCommands
{
    public class AssignTestForCoachCommand
    {
        public class Request: IRequest<Response>
        {
            public Guid TestId { get; set; }
            public Guid CoachId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly UserManager<User> _userManager;

            public Handler(IApplicationDbContext context, UserManager<User> userManager)
            {
                _userManager = userManager;
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var test = await _context.Tests.FirstOrDefaultAsync(x => x.Id == request.TestId);

                if(test is null)
                {
                    throw new NotFoundException($"Test with id {request.TestId}");
                }

                if(test.SpeakingMark.HasValue && test.EssayMark.HasValue && string.IsNullOrWhiteSpace(test.Comment))
                {
                    throw new ValidationException("This test has already checked!");
                }

                var coach = await _userManager.FindByIdAsync(request.CoachId.ToString());

                if (coach is null || !(await _userManager.GetRolesAsync(coach)).Contains("Coach"))
                {
                    throw new ValidationException($"User is either not found or not coach");
                }

                test.CoachId = request.CoachId;

                await _context.SaveChangesAsync(cancellationToken);

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
