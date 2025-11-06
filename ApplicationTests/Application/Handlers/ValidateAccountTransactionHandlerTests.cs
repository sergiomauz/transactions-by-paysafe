using Moq;
using Shouldly;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application;
using Application.Infrastructure.ExternalServices;
using Application.UseCases.AccountTransactions.Commands.ValidateAccountTransaction;
using ApplicationTests.Application.ValidSpecimens;

namespace ApplicationTests.Application.Handlers
{
    public class ValidateAccountTransactionHandlerTests : BaseFormatValidationTest
    {
        private readonly Mock<IOptions<ApplicationSettings>> _settings;
        private readonly Mock<ILogger<ValidateAccountTransactionHandler>> _logger;
        private readonly Mock<IKafkaClientConsumerService> _kafkaClientConsumerService;
        private readonly Mock<IKafkaClientProducerService> _kafkaClientProducerService;
        private readonly Mock<IApiTransactionsService> _apiTransactionsService;

        public ValidateAccountTransactionHandlerTests() : base()
        {
            _settings = new Mock<IOptions<ApplicationSettings>>();
            _logger = new Mock<ILogger<ValidateAccountTransactionHandler>>(); ;
            _kafkaClientConsumerService = new Mock<IKafkaClientConsumerService>();
            _kafkaClientProducerService = new Mock<IKafkaClientProducerService>();
            _apiTransactionsService = new Mock<IApiTransactionsService>();
        }

        [Theory]
        [ClassData(typeof(ValidateAccountTransactionCommandVs))]
        public async Task ValidateAccountTransactionCommand_Ok_Approved(ValidateAccountTransactionCommand command)
        {
            #region ARRANGE      

            // Fake required objects
            var rawMessage = """
                {
                	"TransactionId": "b312b3f7-98e1-40f9-9400-553dbd3c1b7a",
                	"SourceAccountId": "0055ea39-0bf2-4556-8f58-4f17248c936d",
                	"TransactionDate": "2025-11-06 02:39:51",
                	"AccumulatedValue": 0.0,
                	"CurrentValue": 120
                }
                """;

            // Moq 'ApplicationSettings'
            _settings.Invocations.Clear();
            _settings.Setup(s => s.Value).Returns(new ApplicationSettings
            {
                Timezone = -5,
                TopAccumulatedValueByDay = 20500,
                TopCurrentValue = 2500
            });

            // Moq 'IKafkaClientConsumerService'
            _kafkaClientConsumerService.Invocations.Clear();
            _kafkaClientConsumerService
                .Setup(x => x.ConsumeAccountTransaction())
                .Returns(rawMessage);

            // Moq 'IApiTransactionsService'
            _apiTransactionsService.Invocations.Clear();
            _apiTransactionsService
                .Setup(x => x.UpdateAccountTransactionAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((int)HttpStatusCode.OK);

            // Create handler
            var handler = NewDefaultValidateAccountTransactionHandler();
            #endregion

            #region ACT & ASSERT
            await Should.NotThrowAsync(
                async () => await handler.Handle(command, CancellationToken.None)
            );

            _kafkaClientConsumerService.Verify(
                x => x.ConsumeAccountTransaction(),
                Times.Exactly(1)
            );
            _apiTransactionsService.Verify(
                x => x.UpdateAccountTransactionAsync(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(ValidateAccountTransactionCommandVs))]
        public async Task ValidateAccountTransactionCommand_Ok_Rejected(ValidateAccountTransactionCommand command)
        {
            #region ARRANGE      

            // Fake required objects
            var rawMessage = """
                {
                	"TransactionId": "b312b3f7-98e1-40f9-9400-553dbd3c1b7a",
                	"SourceAccountId": "0055ea39-0bf2-4556-8f58-4f17248c936d",
                	"TransactionDate": "2025-11-06 02:39:51",
                	"AccumulatedValue": 20500.0,
                	"CurrentValue": 120
                }
                """;

            // Moq 'ApplicationSettings'
            _settings.Invocations.Clear();
            _settings.Setup(s => s.Value).Returns(new ApplicationSettings
            {
                Timezone = -5,
                TopAccumulatedValueByDay = 20500,
                TopCurrentValue = 2500
            });

            // Moq 'IKafkaClientConsumerService'
            _kafkaClientConsumerService.Invocations.Clear();
            _kafkaClientConsumerService
                .Setup(x => x.ConsumeAccountTransaction())
                .Returns(rawMessage);

            // Moq 'IApiTransactionsService'
            _apiTransactionsService.Invocations.Clear();
            _apiTransactionsService
                .Setup(x => x.UpdateAccountTransactionAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((int)HttpStatusCode.OK);

            // Create handler
            var handler = NewDefaultValidateAccountTransactionHandler();
            #endregion

            #region ACT & ASSERT
            await Should.NotThrowAsync(
                async () => await handler.Handle(command, CancellationToken.None)
            );

            _kafkaClientConsumerService.Verify(
                x => x.ConsumeAccountTransaction(),
                Times.Exactly(1)
            );
            _apiTransactionsService.Verify(
                x => x.UpdateAccountTransactionAsync(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(ValidateAccountTransactionCommandVs))]
        public async Task ValidateAccountTransactionCommand_Fails_EmptyMessage(ValidateAccountTransactionCommand command)
        {
            #region ARRANGE      

            // Fake required objects
            var rawMessage = "";

            // Moq 'IKafkaClientConsumerService'
            _kafkaClientConsumerService.Invocations.Clear();
            _kafkaClientConsumerService
                .Setup(x => x.ConsumeAccountTransaction())
                .Returns(rawMessage);

            // Create handler
            var handler = NewDefaultValidateAccountTransactionHandler();
            #endregion

            #region ACT & ASSERT
            await Should.NotThrowAsync(
                async () => await handler.Handle(command, CancellationToken.None)
            );

            _kafkaClientConsumerService.Verify(
                x => x.ConsumeAccountTransaction(),
                Times.Exactly(1)
            );
            _apiTransactionsService.Verify(
                x => x.UpdateAccountTransactionAsync(It.IsAny<string>(), It.IsAny<int>()),
                Times.Exactly(0)
            );
            _kafkaClientProducerService.Verify(
                x => x.PublishAccountTransactionFailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                            It.IsAny<decimal>(), It.IsAny<decimal>()),
                Times.Exactly(0)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(ValidateAccountTransactionCommandVs))]
        public async Task ValidateAccountTransactionCommand_Fails_MalformedMessage(ValidateAccountTransactionCommand command)
        {
            #region ARRANGE      

            // Fake required objects
            var rawMessage = "{}";

            // Moq 'IKafkaClientConsumerService'
            _kafkaClientConsumerService.Invocations.Clear();
            _kafkaClientConsumerService
                .Setup(x => x.ConsumeAccountTransaction())
                .Returns(rawMessage);

            // Moq 'IKafkaClientProducerService'
            _kafkaClientProducerService.Invocations.Clear();
            _kafkaClientProducerService
                .Setup(x => x.PublishAccountTransactionFailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                            It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync(true);

            // Create handler
            var handler = NewDefaultValidateAccountTransactionHandler();
            #endregion

            #region ACT & ASSERT
            await Should.NotThrowAsync(
                async () => await handler.Handle(command, CancellationToken.None)
            );

            _kafkaClientConsumerService.Verify(
                x => x.ConsumeAccountTransaction(),
                Times.Exactly(1)
            );
            _kafkaClientProducerService.Verify(
                x => x.PublishAccountTransactionFailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                            It.IsAny<decimal>(), It.IsAny<decimal>()),
                Times.Exactly(1)
            );
            #endregion
        }

        private ValidateAccountTransactionHandler NewDefaultValidateAccountTransactionHandler()
        {
            return new ValidateAccountTransactionHandler(_settings.Object, _logger.Object,
                                                            _kafkaClientConsumerService.Object,
                                                            _kafkaClientProducerService.Object,
                                                            _apiTransactionsService.Object);
        }
    }
}
