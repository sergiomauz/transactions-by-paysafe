using System.Net;
using System.Text.Json;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;
using Commons.Enums;
using Application.Infrastructure.ExternalServices;

namespace Application.UseCases.AccountTransactions.Commands.ValidateAccountTransaction
{
    public class ValidateAccountTransactionHandler :
        IRequestHandler<ValidateAccountTransactionCommand>
    {
        private readonly ApplicationSettings _settings;
        private readonly ILogger<ValidateAccountTransactionHandler> _logger;
        private readonly IKafkaClientConsumerService _kafkaClientConsumerService;
        private readonly IKafkaClientProducerService _kafkaClientProducerService;
        private readonly IApiTransactionsService _apiTransactionsService;

        public ValidateAccountTransactionHandler(
            IOptions<ApplicationSettings> settings,
            ILogger<ValidateAccountTransactionHandler> logger,
            IKafkaClientConsumerService kafkaClientConsumerService,
            IKafkaClientProducerService kafkaClientProducerService,
            IApiTransactionsService apiTransactionsService)
        {
            _settings = settings.Value;
            _logger = logger;
            _kafkaClientConsumerService = kafkaClientConsumerService;
            _kafkaClientProducerService = kafkaClientProducerService;
            _apiTransactionsService = apiTransactionsService;
        }

        public async Task Handle(ValidateAccountTransactionCommand command, CancellationToken cancellationToken)
        {
            var rawMessage = _kafkaClientConsumerService.ConsumeAccountTransaction();
            _logger.LogInformation($"Transaction consumed...");

            // If message is not empty, continue
            if (!string.IsNullOrEmpty(rawMessage))
            {
                using (var json = JsonDocument.Parse(rawMessage))
                {
                    // Initialize values
                    var transactionId = Guid.Empty;
                    var sourceAccountId = Guid.Empty;
                    var transactionDate = DateTime.MinValue;
                    var accumulatedValue = decimal.MinValue;
                    var currentValue = decimal.MinValue;
                    var malformedMessage = false;

                    // Exists required and valid fields?
                    var existTransactionId = json.RootElement.TryGetProperty("TransactionId", out var jsonTransactionId);
                    if (!existTransactionId)
                    {
                        malformedMessage = true;
                        _logger.LogCritical("This message does not have a Transaction ID.");
                    }
                    else if (!jsonTransactionId.TryGetGuid(out transactionId))
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a valid Transaction ID: {transactionId}");
                    }
                    var existSourceAccountId = json.RootElement.TryGetProperty("SourceAccountId", out var jsonSourceAccountId);
                    if (!existSourceAccountId)
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a Source Account ID. Transaction ID: {transactionId}");
                    }
                    else if (!jsonSourceAccountId.TryGetGuid(out sourceAccountId))
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a valid Source Account ID. Transaction ID: {transactionId}");
                    }
                    var existTransactionDate = json.RootElement.TryGetProperty("TransactionDate", out var jsonTransactionDate);
                    if (!existTransactionDate)
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a Transaction Date. Transaction ID: {transactionId}");
                    }
                    else if (!DateTime.TryParseExact(jsonTransactionDate.GetString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out transactionDate))
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a valid Transaction Date. Transaction ID: {transactionId}");
                    }
                    var existAccumulatedValue = json.RootElement.TryGetProperty("AccumulatedValue", out var jsonAccumulatedValue);
                    if (!existAccumulatedValue)
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have an Accumulated Transactions Value. Transaction ID: {transactionId}");
                    }
                    else if (!jsonAccumulatedValue.TryGetDecimal(out accumulatedValue))
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a valid Accumulated Transactions Value. Transaction ID: {transactionId}");
                    }
                    var existCurrentValue = json.RootElement.TryGetProperty("CurrentValue", out var jsonCurrentValue);
                    if (!existCurrentValue)
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a Transaction Value. Transaction ID: {transactionId}");
                    }
                    else if (!jsonCurrentValue.TryGetDecimal(out currentValue))
                    {
                        malformedMessage = true;
                        _logger.LogCritical($"This message does not have a valid Transaction Value. Transaction ID: {transactionId}");
                    }

                    // Validate a malformed message
                    if (malformedMessage)
                    {
                        // Send to fails topic if there is a malformed message
                        var failTopicPublished = await _kafkaClientProducerService
                                                        .PublishAccountTransactionFailAsync(transactionId.ToString(), sourceAccountId.ToString(),
                                                                                            transactionDate.ToString("yyyy-MM-dd HH:mm:ss"), accumulatedValue,
                                                                                            currentValue);
                        if (failTopicPublished)
                        {
                            _logger.LogCritical($"Malformed message was not processed. It was sent to 'fails' list in Kafka. \nMessage: {rawMessage}.");
                        }
                        else
                        {
                            _logger.LogCritical($"Malformed message was not processed. Check out the status in the database.\nMessage: {rawMessage}.");
                        }
                        return;
                    }

                    // Validate transaction
                    var validateAccountTransaction = AccountTransactionStatus.Rejected;
                    if (currentValue <= _settings.TopCurrentValue && accumulatedValue + currentValue <= _settings.TopAccumulatedValueByDay)
                    {
                        validateAccountTransaction = AccountTransactionStatus.Approved;
                    }

                    // Update transaction or send to fail topic. If fails, send to fails topic
                    var httpResponseStatus = await _apiTransactionsService.UpdateAccountTransactionAsync(transactionId.ToString(), (int)validateAccountTransaction);
                    if (httpResponseStatus != (int)HttpStatusCode.OK)
                    {
                        var failTopicPublished = await _kafkaClientProducerService
                                                        .PublishAccountTransactionFailAsync(transactionId.ToString(), sourceAccountId.ToString(),
                                                                                            transactionDate.ToString("yyyy-MM-dd HH:mm:ss"), accumulatedValue,
                                                                                            currentValue);
                        if (failTopicPublished)
                        {
                            _logger.LogCritical($"Transaction with ID '{transactionId}' was not processed and sent to 'fails' list in Kafka.");
                        }
                        else
                        {
                            _logger.LogCritical($"Transaction with ID '{transactionId}' was not processed. Check out the status in the database.\nMessage: {rawMessage}.");
                        }
                        return;
                    }

                    // All is ok
                    _logger.LogInformation($"Transaction with ID '{transactionId}' was processed and {validateAccountTransaction.GetEnumDescription()}.");
                }
            }
        }
    }
}
