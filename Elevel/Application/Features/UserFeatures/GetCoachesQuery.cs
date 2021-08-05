using AutoMapper;
using Elevel.Application.Extensions;
using Elevel.Application.Interfaces;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.UserFeatures
{
    public class GetCoachesQuery
    {
        public class Request : IRequest<Response>
        {
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;
            private readonly IApplicationDbContext _context;

            public Handler(IMapper mapper, UserManager<User> userManager, IApplicationDbContext context)
            {
                _mapper = mapper;
                _userManager = userManager;
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var coaches = await _userManager.Users.WhereAsync(async x => (await _userManager.GetRolesAsync(x)).Contains(UserRole.Coach.ToString()));

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    coaches = coaches.Where(x => x.LastName.Contains(request.Name, StringComparison.OrdinalIgnoreCase)
                    || x.FirstName.Contains(request.Name, StringComparison.OrdinalIgnoreCase)
                    || x.UserName.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                }

                var coachList = _mapper.Map<List<CoachDto>>(coaches);

                var tests = _context.Tests.Where(x => x.CoachId.HasValue).AsNoTracking();

                foreach (var coach in coachList)
                {
                    coach.TestCount = tests.Where(x => x.CoachId == coach.Id).Count();
                }

                return new Response
                {
                    Coaches = coachList 
                };
            }
        }

        public class Response
        {
            public IEnumerable<CoachDto> Coaches { get; set; }
        }
        public class CoachDto 
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string UserName { get; set; }

            public int TestCount { get; set; }
        }

    }
}
