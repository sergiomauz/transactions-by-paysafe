using Domain.Entities.Bases;


namespace Application.Infrastructure.Persistence.Bases
{
    public interface IBaseWithGuidRepository<T> where T : BaseEntityWithGuid
    {
        Task<T?> CreateAsync(T entity);
        Task<int> DeleteAsync(IEnumerable<Guid> ids);
        Task<T?> UpdateAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
    }
}
