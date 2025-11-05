using Application.Infrastructure.Persistence;
using Domain.Entities;
using Persistence.Repositories.Bases;

namespace Persistence.Repositories
{
    public class AccountsRepository : BaseWithGuidRepository<Account>, IAccountsRepository
    {
        private readonly PostgresDbContext _postgresDbContext;

        public AccountsRepository(PostgresDbContext postgresDbContext) : base(postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }
    }
}
