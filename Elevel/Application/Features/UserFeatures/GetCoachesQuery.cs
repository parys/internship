using AutoMapper;
using AutoMapper.QueryableExtensions;
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
                var coaches = _mapper.Map<List<CoachDto>>(await _userManager.GetUsersInRoleAsync(nameof(UserRole.Coach)));

                var tests = _context.Tests.Where(x => x.CoachId.HasValue && !x.EssayMark.HasValue).AsNoTracking();

                foreach (var coach in coaches)
                {
                    coach.TestAmount = tests.Where(x => x.CoachId == coach.UserId).Count();
                }


                return new Response
                {
                    Coaches = coaches
                };
            }
        }

        public class Response
        {
            public IEnumerable<CoachDto> Coaches { get; set; }
        }
        public class CoachDto 
        {
            public Guid UserId { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string UserName { get; set; }

            public int TestAmount { get; set; }
        }

    }
}
