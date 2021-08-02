using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Elevel.Application.Infrastructure;
using Elevel.Application.Features.TopicCommands;

namespace Elevel.Application.Infrastructure
{
    public static class Registration
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            AssemblyScanner.FindValidatorsInAssembly(typeof(RequestValidationBehavior<,>).Assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
        }
    }
}