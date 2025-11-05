using System.Net;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MediatR;
using Commons.Enums;
using Application.Commons.Exceptions;
using Application.ErrorCatalog;
using Application.Infrastructure.Persistence;

namespace Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction
{
    public class UpdateAccountTransactionHandler :
        IRequestHandler<UpdateAccountTransactionCommand, UpdateAccountTransactionVm>
    {
        private readonly ILogger<UpdateAccountTransactionHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountTransactionsRepository _accountTransactionsRepository;

        public UpdateAccountTransactionHandler(
            ILogger<UpdateAccountTransactionHandler> logger,
            IMapper mapper,
            IAccountTransactionsRepository accountTransactionsRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _accountTransactionsRepository = accountTransactionsRepository;
        }

        public async Task<UpdateAccountTransactionVm> Handle(UpdateAccountTransactionCommand command, CancellationToken cancellationToken)
        {
            // Verify if transaction exists
            var existingTransaction = await _accountTransactionsRepository.GetByIdAsync(Guid.Parse(command.Id));
            if (existingTransaction == null)
            {
                var handledError = ErrorConstants.UpdateAccountTransactionContent00001;
                var errorMessageArgs = new string[] { command.Id };
                var errorMessage = string.Format(handledError.ErrorMessage, errorMessageArgs);

                _logger.LogWarning(errorMessage);

                throw new ContentValidationException(
                            handledError.PropertyName,
                            handledError.ErrorCode,
                            errorMessage,
                            HttpStatusCode.NotFound);
            }

            // Verify if status is not "Pending"
            if (existingTransaction.AccountTransactionStatus != AccountTransactionStatus.Pending)
            {
                var handledError = ErrorConstants.UpdateAccountTransactionContent00002;
                var errorMessageArgs = new string[] { command.Id };
                var errorMessage = string.Format(handledError.ErrorMessage, errorMessageArgs);

                _logger.LogWarning(errorMessage);

                throw new ContentValidationException(
                            handledError.PropertyName,
                            handledError.ErrorCode,
                            errorMessage,
                            HttpStatusCode.Conflict);
            }
            existingTransaction.AccountTransactionStatus = (AccountTransactionStatus)(command.AccountTransactionStatus);

            // Save transaction
            var newTransaction = await _accountTransactionsRepository.UpdateAsync(existingTransaction);

            // Map 'newTransaction' to response
            var response = _mapper.Map<UpdateAccountTransactionVm>(newTransaction);

            //
            return response;
        }
    }
}
