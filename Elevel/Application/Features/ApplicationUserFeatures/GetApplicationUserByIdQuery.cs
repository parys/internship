using AutoMapper;
using Elevel.Application.Pagination;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.ApplicationUserFeatures
{
    public class GetApplicationUserByIdQuery
    {

        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {

            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

                if(user == null)
                {
                    return new Response();
                }

                return new Response()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreationDate = user.CreationDate,
                    Avatar = user.Avatar,
                    Email = user.Email                 
                };


            }
        }

        [Serializable]
        public class Response : UserDto
        {
        }

        [Serializable]
        public class UserDto
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public string Avatar { get; set; }
            public string Email { get; set; }
        }
    }
}
