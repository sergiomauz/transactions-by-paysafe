using AutoMapper;
using Moq;
using Shouldly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Commons.Enums;
using Domain.Entities;
using Application;
using Application.Commons.Exceptions;
using Application.Infrastructure.ExternalServices;
using Application.Infrastructure.Persistence;
using Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction;
using ApplicationTests.Application.ValidSpecimens;

namespace ApplicationTests.Application.Handlers
{
    public class CreateAccountTransactionHandlerTests : BaseFormatValidationTest
    {
        private readonly Mock<IOptions<ApplicationSettings>> _settings;
        private readonly Mock<ILogger<CreateAccountTransactionHandler>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUowDatabaseTransaction> _uowDatabaseTransaction;
        private readonly Mock<IAccountsRepository> _accountsRepository;
        private readonly Mock<IAccountTransactionsRepository> _accountTransactionsRepository;
        private readonly Mock<IKafkaClientProducerService> _kafkaClientProducerService;

        public CreateAccountTransactionHandlerTests() : base()
        {
            _settings = new Mock<IOptions<ApplicationSettings>>();
            _logger = new Mock<ILogger<CreateAccountTransactionHandler>>(); ;
            _mapper = new Mock<IMapper>();
            _accountsRepository = new Mock<IAccountsRepository>();
            _accountTransactionsRepository = new Mock<IAccountTransactionsRepository>();
            _kafkaClientProducerService = new Mock<IKafkaClientProducerService>();
            _uowDatabaseTransaction = new Mock<IUowDatabaseTransaction>();
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public async Task CreateAccountTransactionCommand_Ok_AccountsC2C(CreateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 0;

            // Fake required objects
            var newTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.Parse(command.SourceAccountId),
                TargetAccountId = Guid.Parse(command.TargetAccountId),
                TransferType = (TransferType)(command.TransferTypeId),
                TicketValidator = command.TicketValidator,
                Amount = command.Amount,
                AccountTransactionStatus = AccountTransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            var sourceAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };
            var targetAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };

            // Moq 'IMapper'
            _mapper.Invocations.Clear();
            _mapper
                .Setup(m => m.Map<CreateAccountTransactionVm>(It.IsAny<AccountTransaction>()))
                .Returns(new CreateAccountTransactionVm { });

            // Moq 'ApplicationSettings'
            _settings.Invocations.Clear();
            _settings.Setup(s => s.Value).Returns(new ApplicationSettings
            {
                Timezone = -5
            });

            // Moq 'IUowDatabaseTransaction'
            _uowDatabaseTransaction.Invocations.Clear();
            _uowDatabaseTransaction
                .Setup(x => x.BeginTransactionAsync())
                .Returns(Task.CompletedTask);
            _uowDatabaseTransaction
                .Setup(x => x.CommitTransactionAsync())
                .Returns(Task.CompletedTask);
            _uowDatabaseTransaction
                .Setup(x => x.RollbackTransactionAsync())
                .Returns(Task.CompletedTask);

            // Moq 'IAccountsRepository'
            _accountsRepository.Invocations.Clear();
            _accountsRepository
                .SetupSequence(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(sourceAccount)
                .ReturnsAsync(targetAccount);

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.CreateAsync(It.IsAny<AccountTransaction>()))
                .ReturnsAsync(newTransaction);
            _accountTransactionsRepository
                .Setup(x => x.GetAccumulatedByDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<double>()))
                .ReturnsAsync(20500);

            // Moq 'IKafkaClientProducerService'
            _kafkaClientProducerService.Invocations.Clear();
            _kafkaClientProducerService
                .Setup(x => x.PublishAccountTransactionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                                It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync(true);

            // Create handler
            var handler = NewDefaultCreateAccountTransactionHandler();
            #endregion

            #region ACT      
            var vm = await handler.Handle(command, CancellationToken.None);
            #endregion

            #region ASSERT      			            
            vm.ShouldBeOfType<CreateAccountTransactionVm>();

            _uowDatabaseTransaction.Verify(
                x => x.BeginTransactionAsync(),
                Times.Exactly(1)
            );
            _uowDatabaseTransaction.Verify(
                x => x.CommitTransactionAsync(),
                Times.Exactly(1)
            );
            _accountsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(2)
            );
            _accountTransactionsRepository.Verify(
                x => x.CreateAsync(It.IsAny<AccountTransaction>()),
                Times.Exactly(1)
            );
            _accountTransactionsRepository.Verify(
                x => x.GetAccumulatedByDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<double>()),
                Times.Exactly(1)
            );
            _kafkaClientProducerService.Verify(
                x => x.PublishAccountTransactionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                                It.IsAny<decimal>(), It.IsAny<decimal>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public async Task CreateAccountTransactionCommand_Fails_SourceAccountNoExists(CreateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 0;

            // Moq 'IAccountsRepository'
            _accountsRepository.Invocations.Clear();
            _accountsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Account)null);

            // Create handler
            var handler = NewDefaultCreateAccountTransactionHandler();
            #endregion

            #region ACT & ASSERT 
            await Should.ThrowAsync<ContentValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );
            _accountsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public async Task CreateAccountTransactionCommand_Fails_TargetAccountNoExists(CreateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 0;

            // Fake required objects
            var sourceAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };

            // Moq 'IAccountsRepository'
            _accountsRepository.Invocations.Clear();
            _accountsRepository
                .SetupSequence(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(sourceAccount)
                .ReturnsAsync((Account)null);

            // Create handler
            var handler = NewDefaultCreateAccountTransactionHandler();
            #endregion

            #region ACT & ASSERT 
            await Should.ThrowAsync<ContentValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );
            _accountsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(2)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public async Task CreateAccountTransactionCommand_Fails_Duplicated(CreateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 0;

            // Fake required objects
            var sourceAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };
            var targetAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };
            var duplicatedTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.Parse(command.SourceAccountId),
                TargetAccountId = Guid.Parse(command.TargetAccountId),
                TransferType = (TransferType)(command.TransferTypeId),
                TicketValidator = command.TicketValidator,
                Amount = command.Amount,
                AccountTransactionStatus = AccountTransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            // Moq 'IAccountsRepository'
            _accountsRepository.Invocations.Clear();
            _accountsRepository
                .SetupSequence(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(sourceAccount)
                .ReturnsAsync(targetAccount);

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.GetDuplicatedTransactionAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync(duplicatedTransaction);

            // Create handler
            var handler = NewDefaultCreateAccountTransactionHandler();
            #endregion

            #region ACT && ASSERT 
            await Should.ThrowAsync<ContentValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            _accountsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(2)
            );
            _accountTransactionsRepository.Verify(
                x => x.GetDuplicatedTransactionAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<long>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public async Task CreateAccountTransactionCommand_Fails_KafkaClient(CreateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 0;

            // Fake required objects
            var newTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.Parse(command.SourceAccountId),
                TargetAccountId = Guid.Parse(command.TargetAccountId),
                TransferType = (TransferType)(command.TransferTypeId),
                TicketValidator = command.TicketValidator,
                Amount = command.Amount,
                AccountTransactionStatus = AccountTransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            var sourceAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };
            var targetAccount = new Account
            {
                Id = Guid.NewGuid(),
                Code = Faker.Random.AlphaNumeric(11),
                Name = $"{Faker.Name.LastName()}, {Faker.Name.FirstName()}",
                Email = Faker.Internet.ExampleEmail()
            };

            // Moq 'IMapper'
            _mapper.Invocations.Clear();
            _mapper
                .Setup(m => m.Map<CreateAccountTransactionVm>(It.IsAny<AccountTransaction>()))
                .Returns(new CreateAccountTransactionVm { });

            // Moq 'ApplicationSettings'
            _settings.Invocations.Clear();
            _settings.Setup(s => s.Value).Returns(new ApplicationSettings
            {
                Timezone = -5
            });

            // Moq 'IUowDatabaseTransaction'
            _uowDatabaseTransaction.Invocations.Clear();
            _uowDatabaseTransaction
                .Setup(x => x.BeginTransactionAsync())
                .Returns(Task.CompletedTask);
            _uowDatabaseTransaction
                .Setup(x => x.CommitTransactionAsync())
                .Returns(Task.CompletedTask);
            _uowDatabaseTransaction
                .Setup(x => x.RollbackTransactionAsync())
                .Returns(Task.CompletedTask);

            // Moq 'IAccountsRepository'
            _accountsRepository.Invocations.Clear();
            _accountsRepository
                .SetupSequence(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(sourceAccount)
                .ReturnsAsync(targetAccount);

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.CreateAsync(It.IsAny<AccountTransaction>()))
                .ReturnsAsync(newTransaction);
            _accountTransactionsRepository
                .Setup(x => x.GetAccumulatedByDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<double>()))
                .ReturnsAsync(20500);

            // Moq 'IKafkaClientProducerService'
            _kafkaClientProducerService.Invocations.Clear();
            _kafkaClientProducerService
                .Setup(x => x.PublishAccountTransactionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                                It.IsAny<decimal>(), It.IsAny<decimal>()))
                .ReturnsAsync(false);

            // Create handler
            var handler = NewDefaultCreateAccountTransactionHandler();
            #endregion

            #region ACT && ASSERT   
            await Should.ThrowAsync<Exception>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            _uowDatabaseTransaction.Verify(
                x => x.BeginTransactionAsync(),
                Times.Exactly(1)
            );
            _uowDatabaseTransaction.Verify(
                x => x.CommitTransactionAsync(),
                Times.Exactly(0)
            );
            _uowDatabaseTransaction.Verify(
                x => x.RollbackTransactionAsync(),
                Times.Exactly(1)
            );
            _accountsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(2)
            );
            _accountTransactionsRepository.Verify(
                x => x.CreateAsync(It.IsAny<AccountTransaction>()),
                Times.Exactly(1)
            );
            _accountTransactionsRepository.Verify(
                x => x.GetAccumulatedByDateAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<double>()),
                Times.Exactly(1)
            );
            _kafkaClientProducerService.Verify(
                x => x.PublishAccountTransactionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                                                                It.IsAny<decimal>(), It.IsAny<decimal>()),
                Times.Exactly(1)
            );
            #endregion
        }

        private CreateAccountTransactionHandler NewDefaultCreateAccountTransactionHandler()
        {
            return new CreateAccountTransactionHandler(_settings.Object, _logger.Object, _mapper.Object,
                                                    _accountsRepository.Object,
                                                    _accountTransactionsRepository.Object,
                                                    _kafkaClientProducerService.Object,
                                                    _uowDatabaseTransaction.Object);
        }
    }
}
