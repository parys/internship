using Elevel.Application.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Elevel.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostingEnvironment;

        public CustomExceptionFilterAttribute(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override void OnException(ExceptionContext context)
        {
            var code = HttpStatusCode.BadRequest;
            

            context.HttpContext.Response.ContentType = "application/json";

            if(context.Exception is ValidationException validationException)
            {
                code = HttpStatusCode.UnprocessableEntity;
                context.Result = new JsonResult(validationException.Message);
            }

            if (string.Equals(context.Exception.Message, "Forbidden", StringComparison.CurrentCultureIgnoreCase))
            {
                code = HttpStatusCode.Forbidden;
                context.Result = new JsonResult("Forbidden");
            }

            if(context.Exception is NotFoundException notFoundException)
            {
                code = HttpStatusCode.NotFound;
                context.Result = new JsonResult(notFoundException.Message);
            }

            context.HttpContext.Response.StatusCode = (int)code;

            if (_hostingEnvironment.IsProduction())
            {
                context.Result = new JsonResult(new
                {
                    error = new [] {context.Exception.Message}
                });
            } else {
                context.Result = new JsonResult(new
                {
                    error = new[] { context.Exception.Message },
                    stackTrace = context.Exception.StackTrace
                });
            }

            return;
        }
    }
}
