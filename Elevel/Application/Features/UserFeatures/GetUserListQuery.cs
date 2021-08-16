using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Extensions;
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

namespace Elevel.Application.Features.UserFeatures
{
    public class GetUserListQuery
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

            private readonly UserManager<User> _userManager;

            public Handler(IMapper mapper, UserManager<User> userManager)
            {
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                var users = _userManager.Users.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(request.LastName))
                {
                    users = users.Where(x => x.LastName.StartsWith(request.LastName));
                }
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                {
                    users = users.Where(x => x.FirstName.StartsWith(request.FirstName));
                }
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    users = users.Where(x => x.Email.StartsWith(request.Email));
                }

                Expression<Func<User, object>> sortBy = x => x.FirstName;
                Expression<Func<User, object>> thenBy = x => x.LastName;
                if (!string.IsNullOrWhiteSpace(request.SortOn))
                {
                    if (request.SortOn.Contains(nameof(User.FirstName),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.FirstName;
                        thenBy = x => x.LastName;
                    }
                    else if (request.SortOn.Contains(nameof(User.LastName),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.LastName;
                        thenBy = x => x.FirstName;
                    }
                    else if (request.SortOn.Contains(nameof(User.CreationDate),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.CreationDate;
                        thenBy = x => x.FirstName;
                    }
                    else if (request.SortOn.Contains(nameof(User.Email),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.Email;
                        thenBy = null;
                    }
                    else if (request.SortOn.Contains(nameof(User.UserName),
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        sortBy = x => x.UserName;
                        thenBy = x => x.FirstName;
                    }
                }

                return await users.GetPagedAsync<Response, User, UsersDTO>(request, _mapper, sortBy, thenBy);
            }
        }

        public class Response : PagedResult<UsersDTO>
        {

        }

        public class UsersDTO
        {
            public Guid UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTimeOffset CreationDate { get; set; }
            public string Avatar { get; set; }
            public string Email { get; set; }
        }
    }
}
