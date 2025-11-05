using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AutoMapper;
using MediatR;
using Application.Commons.Exceptions;
using Application.Commons.Utils;


namespace Api.Responses
{
    public class ExceptionResponsesProcess : ExceptionFilterAttribute
    {
        private IMediator _mediator;
        private IMapper _mapper;

        private void HandleInternalServerException(ExceptionContext context)
        {
            var response = new CustomExceptionResponse(
                message: "Internal Server Error, contact with a system administrator."
            );

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
        }

        private void HandleMediatorNullException(ExceptionContext context)
        {
            var response = new CustomExceptionResponse(
                message: "Server can not process the request. Request body is malformed."
            );

            context.Result = new BadRequestObjectResult(response);
            context.ExceptionHandled = true;
        }

        private void HandleNotImplementedException(ExceptionContext context)
        {
            var response = new CustomExceptionResponse(
                message: "Internal Server Error, contact with a system administrator."
            );

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
        }

        private void HandleFormatValidationException(ExceptionContext context)
        {
            var exception = context.Exception as FormatValidationException;
            var errorsList = exception.Errors;
            var hasErrorsList = errorsList.Any();
            var details = hasErrorsList ? errorsList.GetValidationFormatFailures() : null;
            var response = new CustomExceptionResponse(
                message: null,
                exceptions: details
            );

            context.Result = new BadRequestObjectResult(response);
            context.ExceptionHandled = true;
        }

        private void HandleConflictValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ContentValidationException;
            var details = new Dictionary<string, IEnumerable<Dictionary<string, string>>>
            {
                {
                    exception.PropertyName, new List<Dictionary<string, string>>
                    {
                        new Dictionary<string, string>
                        {
                            { "error_code", exception.CodeError },
                            { "error_message", exception.Message }
                        }
                    }
                }
            };
            var response = new CustomExceptionResponse(
                message: null,
                exceptions: details
            );

            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                context.Result = new NotFoundObjectResult(response);
            }
            else if (exception.StatusCode == HttpStatusCode.Forbidden)
            {
                context.Result = new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else if (exception.StatusCode == HttpStatusCode.Unauthorized)
            {
                context.Result = new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else
            {
                context.Result = new ConflictObjectResult(response);
            }
            context.ExceptionHandled = true;
        }

        public override void OnException(ExceptionContext context)
        {
            _mediator = context.HttpContext.RequestServices.GetService<IMediator>();
            _mapper = context.HttpContext.RequestServices.GetService<IMapper>();

            switch (context.Exception)
            {
                case ArgumentNullException
                    when context.Exception.Source.Equals("MediatR"):
                    HandleMediatorNullException(context);
                    break;
                case NotImplementedException:
                    HandleNotImplementedException(context);
                    break;
                case FormatValidationException:
                    HandleFormatValidationException(context);
                    break;
                case ContentValidationException:
                    HandleConflictValidationException(context);
                    break;
                default:
                    HandleInternalServerException(context);
                    break;
            }
        }
    }
}
