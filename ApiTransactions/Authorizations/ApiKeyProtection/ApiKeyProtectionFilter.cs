using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AutoMapper;
using MediatR;
using Application.Auth.ApiKeyProtection;

namespace Api.AccountTransactions.Authorizations.ApiKeyProtection
{
    public class ApiKeyProtectionFilter : IAuthorizationFilter
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ApiKeyProtectionFilter(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var command = _mapper.Map<ApiKeyProtectionCommand>(context.HttpContext.Request);
            var httpStatusCode = _mediator.Send(command).GetAwaiter().GetResult();

            if (httpStatusCode != HttpStatusCode.OK)
            {
                context.Result = new ObjectResult(new { message = "Access denied: invalid credentials, api-key or signature." })
                {
                    StatusCode = (int)httpStatusCode
                };

                return;
            }
        }
    }
}
