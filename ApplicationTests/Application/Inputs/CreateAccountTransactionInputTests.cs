using Bogus;
using Shouldly;
using Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction;
using Unit.Tests.Application.ValidSpecimens;

namespace Unit.Tests.Application.Inputs
{
    public class CreateAccountTransactionInputTests : BaseFormatValidationTest
    {
        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Ok_AccountsC2C(CreateAccountTransactionCommand command)
        {
            // Default valid specimen always returns true, special case for TransferTypeId = 0

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 0;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            result.IsValid.ShouldBeTrue();
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Ok_AccountsB2C(CreateAccountTransactionCommand command)
        {
            // Default valid specimen always returns true, special case for TransferTypeId = 1

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 1;
            command.SourceAccountId = null;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            result.IsValid.ShouldBeTrue();
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Ok_AccountsC2B(CreateAccountTransactionCommand command)
        {
            // Default valid specimen always returns true, special case for TransferTypeId = 2

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = 2;
            command.TargetAccountId = null;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            result.IsValid.ShouldBeTrue();
            #endregion
        }

        [Fact]
        public void CreateAccountTransactionCommand_Fails_Nulls()
        {
            // All fields must not be null, also, if all are nulls, there are 3 errors (Accounts can be nulls)

            #region ARRANGE      
            var command = new CreateAccountTransactionCommand();
            var validator = new CreateAccountTransactionCommandValidator();
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBeFalse(),
                () => result.Errors.Select(x => x.ErrorMessage).Distinct().Count().ShouldBe(3)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Fails_TransferTypeIdNotValidInt(CreateAccountTransactionCommand command)
        {
            // TransferTypeId must be 0, 1 or 2

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.TransferTypeId = Faker.Random.Bool() ?
                                        Faker.Random.Int(3, int.MaxValue) :
                                        Faker.Random.Int(int.MinValue, -1);
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBeFalse(),
                () => result.Errors.Select(x => x.ErrorMessage).Distinct().Count().ShouldBeGreaterThanOrEqualTo(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Fails_AmountLessOrEqualThanZero(CreateAccountTransactionCommand command)
        {
            // Amount less or equal than zero returns 1 error

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.Amount = Faker.Random.Decimal(decimal.MinValue, 0);
            command.TransferTypeId = 0;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBeFalse(),
                () => result.Errors.Select(x => x.ErrorMessage).Distinct().Count().ShouldBe(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Fails_AmountGreaterThan2500(CreateAccountTransactionCommand command)
        {
            // Amount greater or equal than 2501 returns 1 error

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.Amount = Faker.Random.Decimal(2501, decimal.MaxValue);
            command.TransferTypeId = 0;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBeFalse(),
                () => result.Errors.Select(x => x.ErrorMessage).Distinct().Count().ShouldBe(1)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Fails_AccountsNotNullsNotGuids(CreateAccountTransactionCommand command)
        {
            // Account IDs must be valid Guid

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.SourceAccountId = Faker.Random.AlphaNumeric(15);
            command.TargetAccountId = Faker.Random.AlphaNumeric(15);
            command.TransferTypeId = 0;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBeFalse(),
                () => result.Errors.Select(x => x.ErrorMessage).Distinct().Count().ShouldBe(2)
            );
            #endregion
        }

        [Theory]
        [ClassData(typeof(CreateAccountTransactionCommandVs))]
        public void CreateAccountTransactionCommand_Fails_AccountsEquals(CreateAccountTransactionCommand command)
        {
            // Account IDs must be differents

            #region ARRANGE      
            var validator = new CreateAccountTransactionCommandValidator();
            command.SourceAccountId = command.TargetAccountId;
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            ShouldSatisfyAllConditionsTestExtensions.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBeFalse(),
                () => result.Errors.Select(x => x.ErrorMessage).Distinct().Count().ShouldBe(1)
            );
            #endregion
        }
    }
}
