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
            var publicKey = context.HttpContext.Request.Headers["X-API-KEY"].FirstOrDefault();
            var signature = context.HttpContext.Request.Headers["X-SIGNATURE"].FirstOrDefault();
            var command = new ApiKeyProtectionCommand
            {
                PublicKey = publicKey,
                Signature = signature
            };


            if (publicKey == null || signature == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }
        }
    }
}
