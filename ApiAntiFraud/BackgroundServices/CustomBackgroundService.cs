using AutoMapper;
using MediatR;

namespace Api.AntiFraud.BackgroundServices
{
    public abstract class CustomBackgroundService : BackgroundService
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;

        protected CustomBackgroundService(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }
    }
}
