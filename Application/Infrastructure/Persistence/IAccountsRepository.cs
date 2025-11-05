using Application.Infrastructure.Persistence.Bases;
using Domain.Entities;

namespace Application.Infrastructure.Persistence
{
    public interface IAccountsRepository : IBaseWithGuidRepository<Account>
    {
    }
}
