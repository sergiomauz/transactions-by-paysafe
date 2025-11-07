using System.Net;
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

        public ApiKeyProtectionFilter(IServiceProvider services)
        {
            _mediator = services.GetService<IMediator>();
            _mapper = services.GetService<IMapper>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var command = _mapper.Map<ApiKeyProtectionCommand>(context.HttpContext.Request);
            var httpStatusCode = _mediator.Send(command).GetAwaiter().GetResult();

            if (httpStatusCode != HttpStatusCode.OK)
            {
                context.HttpContext.Response.StatusCode = (int)httpStatusCode;

                return;
            }
        }
    }
}
