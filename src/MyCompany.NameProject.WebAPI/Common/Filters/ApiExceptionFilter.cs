using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using MyCompany.NameProject.Application.Common.Exceptions;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Common.Exceptions;
//using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MyCompany.NameProject.WebAPI.Common.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        private readonly bool _verboseExceptions;

        public ApiExceptionFilter(IConfiguration configuration)
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(FluentValidation.ValidationException), HandleFluentValidationValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(BadRequestException), HandleBadRequestException },
                { typeof(ApiException), HandleApiException }
            };

            _verboseExceptions = configuration.GetSection("VerboseExceptions").Get<bool>();
        }

        private void HandleFluentValidationValidationException(ExceptionContext context)
        {
            var exception = context.Exception as FluentValidation.ValidationException;
            var errors = new Dictionary<string, string[]>();
            var failureGroups = exception.Errors
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                errors.Add(propertyName, propertyFailures);
            }

            var details = new ValidationProblemDetails(errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleApiException(ExceptionContext context)
        {
            var exception = context.Exception as ApiException;
            var statusCode = StatusCodes.Status500InternalServerError;
            var type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

            statusCode = exception.StatusCode ?? statusCode;

            if (new[]
                {
                    StatusCodes.Status400BadRequest,
                    StatusCodes.Status404NotFound
                }.Contains(statusCode))
            {
                statusCode = StatusCodes.Status412PreconditionFailed;
                type = "https://tools.ietf.org/html/rfc7232#section-4.2";
            }
            else if (!new[]
                {
                    StatusCodes.Status400BadRequest,
                    StatusCodes.Status404NotFound,
                    StatusCodes.Status412PreconditionFailed
                }.Contains(statusCode))
            {
                statusCode = StatusCodes.Status500InternalServerError;
            }

            var details = new ProblemDetails()
            {
                Status = statusCode,
                Type = type,
                Detail = exception.Message,
            };

            if (exception.Debug != null)
            {
                string debugString;
                if (exception.Debug is JsonDocument)
                    debugString = (exception.Debug as JsonDocument).ToString();
                else
                    debugString = exception.Debug.ToString();

                details.Extensions["debug"] = debugString;
            }

            context.Result = new ObjectResult(details)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            //SentrySdk.CaptureException(context.Exception);

            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status500InternalServerError;

            var detail = string.Empty;

            if (_verboseExceptions)
                detail = $"{context.Exception?.Message}\r\n{context.Exception?.StackTrace}";

            var details = new ProblemDetails
            {
                Status = statusCode,
                Title = "An error occurred while processing your request",
                Detail = detail,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var details = new ValidationProblemDetails(exception.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        public IConfiguration Configuration { get; }
    }
}