using AutoFixture.Kernel;
using Bogus;
using Application.UseCases.AccountTransactions.Commands.ValidateAccountTransaction;

namespace ApplicationTests.SpecimenBuilders
{
    public class ValidateAccountTransactionCommandSb : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(ValidateAccountTransactionCommand))
            {
                var faker = new Faker<ValidateAccountTransactionCommand>()
                    .RuleFor(c => c.StoppingToken, _ => CancellationToken.None);

                return faker.Generate();
            }

            return new NoSpecimen();
        }
    }
}
