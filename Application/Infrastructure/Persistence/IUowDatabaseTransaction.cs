namespace Application.Infrastructure.Persistence
{
    public interface IUowDatabaseTransaction : IAsyncDisposable
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task CreateSavepointAsync(string savepointName);
        Task RollbackToSavepointAsync(string savepointName);
        Task ReleaseSavepointAsync(string savepointName);
    }
}
