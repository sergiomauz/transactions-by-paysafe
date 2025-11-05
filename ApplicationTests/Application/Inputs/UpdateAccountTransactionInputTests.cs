using Shouldly;
using Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction;
using ApplicationTests.Application.ValidSpecimens;

namespace ApplicationTests.Application.Inputs
{
    public class UpdateAccountTransactionInputTests : BaseFormatValidationTest
    {
        [Theory]
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public void UpdateAccountTransactionCommand_Ok(UpdateAccountTransactionCommand command)
        {
            // Default valid specimen always returns true

            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            #endregion

            #region ACT      
            var result = validator.Validate(command);
            #endregion

            #region ASSERT      			
            result.IsValid.ShouldBeTrue();
            #endregion
        }

        [Fact]
        public void UpdateAccountTransactionCommand_Fails_Nulls()
        {
            // All fields must not be null, also, if all are nulls, there are 2 errors

            #region ARRANGE      
            var command = new UpdateAccountTransactionCommand();
            var validator = new UpdateAccountTransactionCommandValidator();
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
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public void UpdateAccountTransactionCommand_Fails_IdNotGuid(UpdateAccountTransactionCommand command)
        {
            // Account IDs must be valid Guid

            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.Id = Faker.Random.AlphaNumeric(15);
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
        [ClassData(typeof(UpdateAccountTransactionCommandVs))]
        public void UpdateAccountTransactionCommand_Fails_AccountTransactionStatusNotValidInt(UpdateAccountTransactionCommand command)
        {
            // AccountTransactionStatus must be 1 or 2

            #region ARRANGE      
            var validator = new UpdateAccountTransactionCommandValidator();
            command.AccountTransactionStatus = Faker.Random.Bool() ?
                                                Faker.Random.Int(3, int.MaxValue) :
                                                Faker.Random.Int(int.MinValue, 0);
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
    }
}
