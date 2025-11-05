using AutoFixture.Kernel;
using Bogus;
using ApplicationTests.CustomMocks;
using Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction;

namespace ApplicationTests.SpecimenBuilders
{
    public class CreateAccountTransactionCommandSb : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(CreateAccountTransactionCommand))
            {
                var faker = new Faker<CreateAccountTransactionCommand>()
                    .RuleFor(c => c.SourceAccountId, f => f.Random.Guid().ToString())
                    .RuleFor(c => c.TargetAccountId, f => f.Random.Guid().ToString())
                    .RuleFor(c => c.TransferTypeId, f => f.Random.Int(0, 2))
                    .RuleFor(c => c.Amount, f => f.Random.Decimal(1, 2500))
                    .RuleFor(c => c.TicketValidator, f => f.Random.Long(0, DateTime.MaxValue.Ticks))
                    .RuleFor(c => c.Request, f => new HttpRequestMock().Create().Object);

                return faker.Generate();
            }

            return new NoSpecimen();
        }
    }
}
