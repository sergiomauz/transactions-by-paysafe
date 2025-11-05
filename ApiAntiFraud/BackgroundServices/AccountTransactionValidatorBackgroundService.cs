using AutoMapper;
using MediatR;
using Application.UseCases.AccountTransactions.Commands.ValidateAccountTransaction;

namespace Api.AntiFraud.BackgroundServices
{
    public class AccountTransactionValidatorBackgroundService : CustomBackgroundService
    {
        public AccountTransactionValidatorBackgroundService(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var command = Mapper.Map<ValidateAccountTransactionCommand>(stoppingToken);
                await Mediator.Send(command);

                // Optional, a delay per operation
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
