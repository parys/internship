using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Elevel.Application.Pagination;

namespace Elevel.Application.Features.AudioFeatures
{
    public class GetAuditionListQuery
    {
        public class Request : PagedQueryBase, IRequest<Response>
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
        }
        public class Handler: IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var auditionList = _context.Auditions.AsNoTracking()
                    .Include(c => c.Id)
                    .Include(c => c.AudioFilePath)
                    .Include(c => c.CreationDate)
                    .OrderBy(c => c.CreationDate);

                var audition = new List<AuditionDto>();
                foreach (var item in auditionList)
                {
                    var dto = _mapper.Map<AuditionDto>(item);
                    audition.Add(dto);
                }
                return new Response
                {
                    PageSize = request.PageSize,
                    CurrentPage = request.CurrentPage,
                    Results = audition,
                    RowCount = await auditionList.CountAsync(cancellationToken)
                };
            }
        }

        [Serializable]
        public class Response: PagedResult<AuditionDto>
        {

        }
        [Serializable]
        public class AuditionDto
        {
            public Guid Id { get; set; }
            public string AudioFilePath { get; set; }
            public DateTimeOffset CreationDate { get; set; }
        }
    }
}
