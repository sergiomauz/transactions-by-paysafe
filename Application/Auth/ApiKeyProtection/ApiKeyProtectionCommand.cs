using System.Net;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;
using Application.Commons.Mapping;

namespace Application.Auth.ApiKeyProtection
{
    public class ApiKeyProtectionCommand :
        IMapFrom<HttpRequest>,
        IRequest<HttpStatusCode>
    {
        public HttpRequest Request { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HttpRequest, ApiKeyProtectionCommand>()
                .ForMember(d => d.Request, m => m.MapFrom(o => o));
        }
    }
}
