using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Elevel.Application.Interfaces;
using MediatR;

namespace Elevel.Application.Features.Question
{
    public class ImportQuestionsCommand
    {
        public class Request : IRequest<Response>
        {
            public Stream InputFileStream { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;

            private readonly IMapper _mapper;

            private readonly IQuestionsImporter _questionsImporter;

            public Handler(IApplicationDbContext context, IMapper mapper, IQuestionsImporter questionsImporter)
            {
                _context = context;
                _mapper = mapper;
                _questionsImporter = questionsImporter;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                await _questionsImporter.GetDataAsync(request.InputFileStream);
                return null;
            }
        }


        public class Response
        {
            public int Id { get; set; }
        }
    }
}