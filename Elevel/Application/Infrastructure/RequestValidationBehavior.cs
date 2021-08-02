using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Elevel.Application.Infrastructure
{
    public class RequestValidationBehavior<Request, Response> : IPipelineBehavior<Request, Response>
        where Request : IRequest<Response>
    {
        private readonly IEnumerable<IValidator<Request>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<Request>> validators)
        {
            _validators = validators;
        }

        public Task<Response> Handle(Request request, CancellationToken cancellationToken, RequestHandlerDelegate<Response> next)
        {
            var context = new ValidationContext<Request>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(string.Join("\n", failures));
            }

            return next();
        }
    }
}