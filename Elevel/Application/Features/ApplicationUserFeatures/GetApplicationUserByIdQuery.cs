using AutoMapper;
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
    public class GetApplicationUserByIdQuery
    {

        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Guid Id { get; set; }
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

                foreach (var users in Authorization.DefaultUsers.Values)
                {
                    var user = users.FirstOrDefault(x => x.Id == request.Id);

                    if (user is null) continue;

                    return new Response()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreationDate = user.CreationDate,
                        Avatar = user.Avatar
                    };

                }

                return new Response();
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
        }
    }
}
