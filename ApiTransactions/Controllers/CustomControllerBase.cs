using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Responses;


namespace Api.Controllers
{
    [ExceptionResponsesProcess]
    [OkResponsesProcess]
    public class CustomControllerBase : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;

        protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected IMapper? Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}
