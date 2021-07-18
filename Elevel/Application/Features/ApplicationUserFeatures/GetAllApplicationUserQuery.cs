﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elevel.Application.Pagination;
using Elevel.Domain;
using Elevel.Domain.Models;
using MediatR;
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
            public DateTimeOffset? CreationDate { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {

            private readonly IMapper _mapper;

            public Handler(IMapper mapper)
            {
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var userList = new List<ApplicationUser>();

                foreach (var user in Authorization.DefaultUsers.Values)
                {
                    foreach (var item in user)
                    {
                        userList.Add(item);
                    }
                }

                IQueryable<ApplicationUser> users;

                if (string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate == null)
                {
                    users = userList.OrderBy(u => u.LastName).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate != null)
                {
                    users = userList.Where(x => x.LastName == request.LastName && x.FirstName == request.FirstName && x.CreationDate == request.CreationDate).OrderBy(u => u.LastName).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate == null)
                {
                    users = userList.Where(x => x.LastName == request.LastName && x.FirstName == request.FirstName ).OrderBy(u => u.LastName).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate != null)
                {
                    users = userList.Where(x => x.LastName == request.LastName  && x.CreationDate == request.CreationDate).OrderBy(u => u.LastName).AsQueryable();
                }

                if (string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate != null)
                {
                    users = userList.Where(x =>  x.FirstName == request.FirstName && x.CreationDate == request.CreationDate).OrderBy(u => u.LastName).AsQueryable();
                }

                if (string.IsNullOrWhiteSpace(request.LastName) && string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate != null)
                {
                    users = userList.Where(x => x.CreationDate == request.CreationDate).OrderBy(u => u.LastName).AsQueryable();
                }

                if (string.IsNullOrWhiteSpace(request.LastName) && !string.IsNullOrWhiteSpace(request.FirstName) && request.CreationDate == null)
                {
                    users = userList.Where(x =>x.FirstName == request.FirstName).OrderBy(u => u.LastName).AsQueryable();
                }

                else 
                {
                    users = userList.Where(x => x.LastName == request.LastName).OrderBy(u => u.LastName).AsQueryable();
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
        }
    }
}