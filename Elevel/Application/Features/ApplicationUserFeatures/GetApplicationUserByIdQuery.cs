﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Infrastructure;
using Elevel.Application.Pagination;
using Elevel.Domain.Enums;
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
        public class Request : IRequest<Response>
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

                var applicationUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                var roles = await _userManager.GetRolesAsync(applicationUser);

                var user = _mapper.Map<ApplicationUser,Response>(applicationUser);

                user.Roles = roles;

                if (user == null)
                {
                    throw new NotFoundException(nameof(ApplicationUser));
                }

                return user;
            }

        }
        public class Response
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public string Avatar { get; set; }
            public string Email { get; set; }
            public IList<string> Roles { get; set; }
        }
    }
}
