using Domain.Entities;
using Application.Infrastructure.Persistence.Bases;

namespace Application.Infrastructure.Persistence
{
    public interface IAccountTransactionsRepository : IBaseWithGuidRepository<AccountTransaction>
    {
        Task<AccountTransaction?> GetDuplicatedTransactionAsync(Guid sourceAccountId, Guid targetAccountId, long ticketValidator);
        Task<decimal> GetAccumulatedByDateAsync(Guid sourceAccountId, DateTime transactionDate, double timezone);
    }
}
