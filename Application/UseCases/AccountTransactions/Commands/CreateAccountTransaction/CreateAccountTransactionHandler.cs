using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using MediatR;
using Commons.Enums;
using Domain.Entities;
using Application.Commons.Exceptions;
using Application.ErrorCatalog;
using Application.Infrastructure.ExternalServices;
using Application.Infrastructure.Persistence;

namespace Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction
{
    public class CreateAccountTransactionHandler :
        IRequestHandler<CreateAccountTransactionCommand, CreateAccountTransactionVm>
    {
        private readonly ApplicationSettings _settings;
        private readonly ILogger<CreateAccountTransactionHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountsRepository _accountsRepository;
        private readonly IAccountTransactionsRepository _accountTransactionsRepository;
        private readonly IKafkaClientProducerService _kafkaClientProducerService;
        private readonly IUowDatabaseTransaction _uowDatabaseTransaction;

        public CreateAccountTransactionHandler(
            IOptions<ApplicationSettings> settings,
            ILogger<CreateAccountTransactionHandler> logger,
            IMapper mapper,
            IAccountsRepository accountsRepository,
            IAccountTransactionsRepository accountTransactionsRepository,
            IKafkaClientProducerService kafkaClientProducerService,
            IUowDatabaseTransaction uowDatabaseTransaction)
        {
            _settings = settings.Value;
            _logger = logger;
            _mapper = mapper;
            _accountsRepository = accountsRepository;
            _accountTransactionsRepository = accountTransactionsRepository;
            _kafkaClientProducerService = kafkaClientProducerService;
            _uowDatabaseTransaction = uowDatabaseTransaction;
        }

        public async Task<CreateAccountTransactionVm> Handle(CreateAccountTransactionCommand command, CancellationToken cancellationToken)
        {
            // Validate if Accounts exists
            if (!string.IsNullOrEmpty(command.SourceAccountId))
            {
                var existingSourceAccount = await _accountsRepository.GetByIdAsync(Guid.Parse(command.SourceAccountId));
                if (existingSourceAccount == null)
                {
                    var handledError = ErrorConstants.CreateAccountTransactionContent00001;
                    var errorMessageArgs = new string[] { command.SourceAccountId };
                    var errorMessage = string.Format(handledError.ErrorMessage, errorMessageArgs);

                    _logger.LogCritical(errorMessage);

                    throw new ContentValidationException(
                                handledError.PropertyName,
                                handledError.ErrorCode,
                                errorMessage,
                                HttpStatusCode.Conflict);
                }
            }
            if (!string.IsNullOrEmpty(command.TargetAccountId))
            {
                var existingTargetAccount = await _accountsRepository.GetByIdAsync(Guid.Parse(command.TargetAccountId));
                if (existingTargetAccount == null)
                {
                    var handledError = ErrorConstants.CreateAccountTransactionContent00002;
                    var errorMessageArgs = new string[] { command.TargetAccountId };
                    var errorMessage = string.Format(handledError.ErrorMessage, errorMessageArgs);

                    _logger.LogCritical(errorMessage);

                    throw new ContentValidationException(
                                handledError.PropertyName,
                                handledError.ErrorCode,
                                errorMessage,
                                HttpStatusCode.Conflict);
                }
            }

            // Validate transaction, using a ticket to avoid duplicates from the same client. 
            // An operation with the same ticket is not valid.
            var duplicatedTransaction = await _accountTransactionsRepository.GetDuplicatedTransactionAsync(
                Guid.Parse(command.SourceAccountId),
                Guid.Parse(command.TargetAccountId),
                command.TicketValidator.Value
            );
            if (duplicatedTransaction != null)
            {
                var handledError = ErrorConstants.CreateAccountTransactionContent00003;
                var errorMessageArgs = new string[] { command.SourceAccountId,
                                                        command.TargetAccountId,
                                                        command.TicketValidator.Value.ToString() };
                var errorMessage = string.Format(handledError.ErrorMessage, errorMessageArgs);

                _logger.LogCritical(errorMessage);

                throw new ContentValidationException(
                            handledError.PropertyName,
                            handledError.ErrorCode,
                            errorMessage,
                            HttpStatusCode.Conflict);
            }

            // Begin UoW for database transactions. Commit only if transaction was sent to be processed by Anti-Fraud service
            await _uowDatabaseTransaction.BeginTransactionAsync();

            // Save transaction in DB
            var newTransaction = await _accountTransactionsRepository.CreateAsync(
                new AccountTransaction
                {
                    SourceAccountId = Guid.Parse(command.SourceAccountId),
                    TargetAccountId = Guid.Parse(command.TargetAccountId),
                    TransferType = (TransferType)(command.TransferTypeId),
                    TicketValidator = command.TicketValidator,
                    Amount = command.Amount,
                    AccountTransactionStatus = AccountTransactionStatus.Pending
                }
            );
            _logger.LogInformation($"Transaction '{newTransaction.Id}' was saved.");

            // Get accumulated transaction values by Date. Considering timezone
            var accumulatedApprovedByDate = await _accountTransactionsRepository
                                                    .GetAccumulatedByDateAsync(newTransaction.SourceAccountId.Value,
                                                                            newTransaction.CreatedAt.Value,
                                                                            _settings.Timezone);

            // Publish message in Kafka, to be processed by Anti-Fraud service later, with timezone
            var published = await _kafkaClientProducerService
                                    .PublishAccountTransactionAsync(newTransaction.Id.Value.ToString(),
                                                                    newTransaction.SourceAccountId.Value.ToString(),
                                                                    newTransaction.CreatedAt.Value.AddHours(_settings.Timezone).ToString("yyyy-MM-dd HH:mm:ss"),
                                                                    accumulatedApprovedByDate, newTransaction.Amount.Value);
            _logger.LogInformation($"Transaction '{newTransaction.Id}' was published to Kafka.");

            // If not 'published' in Kafka to be processed, rollback transaction and throw exception
            if (!published)
            {
                await _uowDatabaseTransaction.RollbackTransactionAsync();
                _logger.LogWarning($"Transaction '{newTransaction.Id}' was rollbacked.");

                var handledError = ErrorConstants.CreateAccountTransactionContent00004;
                var errorMessageArgs = new string[] { command.SourceAccountId };
                var errorMessage = string.Format(handledError.ErrorMessage, errorMessageArgs);

                _logger.LogCritical(errorMessage);

                throw new Exception(errorMessage);
            }
            await _uowDatabaseTransaction.CommitTransactionAsync();

            _logger.LogInformation($"Transaction '{newTransaction.Id}' was commited.");

            // Map 'newTransaction' to response
            var response = _mapper.Map<CreateAccountTransactionVm>(newTransaction);

            // 
            return response;
        }
    }
}
