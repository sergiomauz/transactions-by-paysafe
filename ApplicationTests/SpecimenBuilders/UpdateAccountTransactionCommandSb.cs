using AutoFixture.Kernel;
using Bogus;
using Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction;
using Unit.Tests.CustomMocks;

namespace Unit.Tests.SpecimenBuilders
{
    public class UpdateAccountTransactionCommandSb : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(UpdateAccountTransactionCommand))
            {
                var faker = new Faker<UpdateAccountTransactionCommand>()
                    .RuleFor(c => c.Id, f => f.Random.Guid().ToString())
                    .RuleFor(c => c.AccountTransactionStatus, f => f.Random.Int(1, 2))
                    .RuleFor(c => c.Request, f => new HttpRequestMock().Create().Object);

                return faker.Generate();
            }

            return new NoSpecimen();
        }
    }
}
