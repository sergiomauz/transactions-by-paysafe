using Microsoft.EntityFrameworkCore;
using Domain.Entities.Bases;
using Application.Infrastructure.Persistence.Bases;


namespace Persistence.Repositories.Bases
{
    public abstract class BaseWithCodeRepository<T> : BaseWithIdRepository<T>, IBaseWithCodeRepository<T>
        where T : BaseEntityWithIdAndCode
    {
        private readonly PostgresDbContext _sqlServerDbContext;

        public BaseWithCodeRepository(PostgresDbContext sqlServerDbContext) : base(sqlServerDbContext)
        {
            _sqlServerDbContext = sqlServerDbContext;
        }

        public virtual async Task<T?> GetByCodeAsync(string code)
        {
            var entity = await _sqlServerDbContext.Set<T>().FirstOrDefaultAsync(e => e.Code.Equals(code));

            return entity;
        }
    }
}
