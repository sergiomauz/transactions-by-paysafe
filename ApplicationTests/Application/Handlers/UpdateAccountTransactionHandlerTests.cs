using Application.Commons.Exceptions;
using Application.Infrastructure.Persistence;
using Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction;
using ApplicationTests.Application.ValidSpecimens;
using AutoMapper;
using Commons.Enums;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace ApplicationTests.Application.Handlers
{
    public class UpdateAccountTransactionHandlerTests : BaseFormatValidationTest
    {
        private readonly Mock<ILogger<UpdateAccountTransactionHandler>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IAccountTransactionsRepository> _accountTransactionsRepository;

        public UpdateAccountTransactionHandlerTests() : base()
        {
            _logger = new Mock<ILogger<UpdateAccountTransactionHandler>>(); ;
            _mapper = new Mock<IMapper>();
            _accountTransactionsRepository = new Mock<IAccountTransactionsRepository>();
        }

        [Theory]
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public async Task UpdateAccountTransactionCommand_Ok_Approved_C2C(UpdateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.AccountTransactionStatus = 1;

            // Fake required objects
            var existingTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferType = TransferType.C2C,
                TicketValidator = DateTime.UtcNow.AddHours(-5).Ticks,
                Amount = Faker.Random.Decimal(1, 2500),
                AccountTransactionStatus = AccountTransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Moq 'IMapper'
            _mapper.Invocations.Clear();
            _mapper
                .Setup(m => m.Map<UpdateAccountTransactionVm>(It.IsAny<AccountTransaction>()))
                .Returns(new UpdateAccountTransactionVm { });

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingTransaction);
            _accountTransactionsRepository
                .Setup(x => x.UpdateAsync(It.IsAny<AccountTransaction>()))
                .ReturnsAsync(existingTransaction);

            // Create handler
            var handler = NewDefaultUpdateAccountTransactionHandler();
            #endregion

            #region ACT      
            var vm = await handler.Handle(command, CancellationToken.None);
            #endregion

            #region ASSERT      			            
            vm.ShouldBeOfType<UpdateAccountTransactionVm>();

            _accountTransactionsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(1)
            );
            _accountTransactionsRepository.Verify(
                x => x.UpdateAsync(It.IsAny<AccountTransaction>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public async Task UpdateAccountTransactionCommand_Ok_Rejected_C2C(UpdateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.AccountTransactionStatus = 2;

            // Fake required objects
            var existingTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferType = TransferType.C2C,
                TicketValidator = DateTime.UtcNow.AddHours(-5).Ticks,
                Amount = Faker.Random.Decimal(1, 2500),
                AccountTransactionStatus = AccountTransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Moq 'IMapper'
            _mapper.Invocations.Clear();
            _mapper
                .Setup(m => m.Map<UpdateAccountTransactionVm>(It.IsAny<AccountTransaction>()))
                .Returns(new UpdateAccountTransactionVm { });

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingTransaction);
            _accountTransactionsRepository
                .Setup(x => x.UpdateAsync(It.IsAny<AccountTransaction>()))
                .ReturnsAsync(existingTransaction);

            // Create handler
            var handler = NewDefaultUpdateAccountTransactionHandler();
            #endregion

            #region ACT      
            var vm = await handler.Handle(command, CancellationToken.None);
            #endregion

            #region ASSERT      			            
            vm.ShouldBeOfType<UpdateAccountTransactionVm>();

            _accountTransactionsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(1)
            );
            _accountTransactionsRepository.Verify(
                x => x.UpdateAsync(It.IsAny<AccountTransaction>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public async Task UpdateAccountTransactionCommand_Fails_TransactionNotFound(UpdateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.AccountTransactionStatus = 1;

            // Create handler
            var handler = NewDefaultUpdateAccountTransactionHandler();
            #endregion

            #region ACT && ASSERT     
            await Should.ThrowAsync<ContentValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            _accountTransactionsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public async Task UpdateAccountTransactionCommand_Fails_NotPossibleFromRejected(UpdateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.AccountTransactionStatus = 2;

            // Fake required objects
            var existingTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferType = TransferType.C2C,
                TicketValidator = DateTime.UtcNow.AddHours(-5).Ticks,
                Amount = Faker.Random.Decimal(1, 2500),
                AccountTransactionStatus = AccountTransactionStatus.Approved,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Moq 'IMapper'
            _mapper.Invocations.Clear();
            _mapper
                .Setup(m => m.Map<UpdateAccountTransactionVm>(It.IsAny<AccountTransaction>()))
                .Returns(new UpdateAccountTransactionVm { });

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingTransaction);

            // Create handler
            var handler = NewDefaultUpdateAccountTransactionHandler();
            #endregion

            #region ACT && ASSERT      
            await Should.ThrowAsync<ContentValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            _accountTransactionsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public async Task UpdateAccountTransactionCommand_Fails_NotPossibleFromApproved(UpdateAccountTransactionCommand command)
        {
            //
            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.AccountTransactionStatus = 1;

            // Fake required objects
            var existingTransaction = new AccountTransaction
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferType = TransferType.C2C,
                TicketValidator = DateTime.UtcNow.AddHours(-5).Ticks,
                Amount = Faker.Random.Decimal(1, 2500),
                AccountTransactionStatus = AccountTransactionStatus.Approved,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Moq 'IMapper'
            _mapper.Invocations.Clear();
            _mapper
                .Setup(m => m.Map<UpdateAccountTransactionVm>(It.IsAny<AccountTransaction>()))
                .Returns(new UpdateAccountTransactionVm { });

            // Moq 'IAccountTransactionsRepository'
            _accountTransactionsRepository.Invocations.Clear();
            _accountTransactionsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingTransaction);

            // Create handler
            var handler = NewDefaultUpdateAccountTransactionHandler();
            #endregion

            #region ACT && ASSERT      
            await Should.ThrowAsync<ContentValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None)
            );

            _accountTransactionsRepository.Verify(
                x => x.GetByIdAsync(It.IsAny<Guid>()),
                Times.Exactly(1)
            );
            #endregion
        }

        private UpdateAccountTransactionHandler NewDefaultUpdateAccountTransactionHandler()
        {
            return new UpdateAccountTransactionHandler(_logger.Object, _mapper.Object, _accountTransactionsRepository.Object);
        }
    }
}
