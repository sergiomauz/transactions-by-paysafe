using System.Collections;
using AutoFixture;
using Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction;
using Unit.Tests.SpecimenBuilders;

namespace Unit.Tests.Application.ValidSpecimens
{
    public class CreateAccountTransactionCommandVs : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new CreateAccountTransactionCommandSb());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            for (var i = 0; i < 10; i++)
            {
                yield return new object[] { fixture.Create<CreateAccountTransactionCommand>() };
            }
        }
    }
}
