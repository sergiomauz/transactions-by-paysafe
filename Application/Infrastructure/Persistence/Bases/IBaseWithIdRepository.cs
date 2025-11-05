using Domain.Entities.Bases;


namespace Application.Infrastructure.Persistence.Bases
{
    public interface IBaseWithIdRepository<T> where T : BaseEntityWithId
    {
        Task<T?> CreateAsync(T entity);
        Task<int> DeleteAsync(List<int> ids);
        Task<T?> UpdateAsync(T entity);
        Task<T?> GetByIdAsync(int id);
    }
}
