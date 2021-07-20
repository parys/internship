﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Interfaces;
using Elevel.Application.Pagination;
using Elevel.Domain;
using Elevel.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elevel.Application.Features.ApplicationUserFeatures
{
    public class GetAllApplicationUserQuery
    {

        public class Request : PagedQueryBase, IRequest<Response>
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Email { get; set; }
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

                IQueryable<ApplicationUser> users = _userManager.Users.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.LastName))
                {
                    users = users.Where(x => x.LastName == request.LastName);
                }
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                {
                    users = users.Where(x =>x.FirstName == request.FirstName);
                }
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    users = users.Where(x => x.Email == request.Email);
                }


                return new Response()
                {
                    CurrentPage = request.CurrentPage,
                    PageSize = request.PageSize,
                    RowCount = await users.CountAsync(),
                    Results = await users.Skip(request.SkipCount()).Take(request.PageSize).ProjectTo<GetApplicationUserByIdQuery.Response>(_mapper.ConfigurationProvider).ToListAsync()
                };
            }
        }

        public class Response : PagedResult<GetApplicationUserByIdQuery.Response>
        {

        }
    }
}
