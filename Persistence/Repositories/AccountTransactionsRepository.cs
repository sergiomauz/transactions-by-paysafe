using Microsoft.EntityFrameworkCore;
using Commons.Enums;
using Domain.Entities;
using Persistence.Repositories.Bases;
using Application.Infrastructure.Persistence;

namespace Persistence.Repositories
{
    public class AccountTransactionsRepository : BaseWithGuidRepository<AccountTransaction>, IAccountTransactionsRepository
    {
        private readonly PostgresDbContext _postgresDbContext;

        public AccountTransactionsRepository(PostgresDbContext postgresDbContext) : base(postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<AccountTransaction?> GetDuplicatedTransactionAsync(Guid sourceAccountId, Guid targetAccountId, long ticketValidator)
        {
            var entity = await _postgresDbContext.Set<AccountTransaction>()
                                                    .SingleOrDefaultAsync(t => t.SourceAccountId == sourceAccountId
                                                                                && t.TargetAccountId == targetAccountId
                                                                                && t.TicketValidator == ticketValidator);

            return entity;
        }

        public async Task<decimal> GetAccumulatedByDateAsync(Guid sourceAccountId, DateTime transactionDate, double timezone)
        {
            // Using timezone compute the accumulated value of transactions by date
            var accumulated = await _postgresDbContext.Set<AccountTransaction>()
                .Where(t =>
                    t.SourceAccountId == sourceAccountId &&
                    t.AccountTransactionStatus == AccountTransactionStatus.Approved &&
                    t.CreatedAt.Value.AddHours(timezone) >= transactionDate.AddHours(timezone).Date &&
                    t.CreatedAt.Value.AddHours(timezone) < transactionDate.AddHours(timezone).Date.AddDays(1))
                .SumAsync(t => t.Amount) ?? 0;

            return accumulated;
        }
    }
}
