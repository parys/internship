using AutoMapper;
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

            private readonly IApplicationDbContext _context;

            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {

                IQueryable<ApplicationUser> users;

                if (!string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && !string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.LastName == request.LastName
                        && x.FirstName == request.FirstName
                        && x.Email == request.Email);
                }
                else if (!string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.LastName == request.LastName
                        && x.FirstName == request.FirstName);
                }
                else if (!string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && !string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.LastName == request.LastName
                        && x.Email == request.Email);
                }
                else if (string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && !string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.FirstName == request.FirstName
                        && x.Email == request.Email);
                }
                else if (!string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.LastName == request.LastName);
                }
                else if (string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && !string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                         x.Email == request.Email);
                }
                else if (string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.FirstName == request.FirstName);
                }
                else if (string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && string.IsNullOrWhiteSpace(request.Email))
                {
                    users = _userManager.Users.AsNoTracking();
                }
                else
                {
                    users = _userManager.Users.AsNoTracking().Where(x =>
                        x.LastName == request.LastName
                        && x.FirstName == request.FirstName
                        && x.Email == request.Email);
                }

                return new Response()
                {
                    CurrentPage = request.CurrentPage,
                    PageSize = request.PageSize,
                    RowCount = users.Count(),
                    Results = users.Skip(request.SkipCount()).Take(request.PageSize).ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToList()
                };
            }
        }

        [Serializable]
        public class Response : PagedResult<UserDto>
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
