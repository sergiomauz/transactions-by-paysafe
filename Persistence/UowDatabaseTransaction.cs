using Microsoft.EntityFrameworkCore.Storage;
using Application.Infrastructure.Persistence;


namespace Persistence
{
    public class UowDatabaseTransaction : IUowDatabaseTransaction
    {
        private readonly PostgresDbContext _sqlServerDbContext;
        private IDbContextTransaction? _currentTransaction;

        public UowDatabaseTransaction(PostgresDbContext sqlServerDbContext)
        {
            _sqlServerDbContext = sqlServerDbContext;
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("A transaction is already in progress.");

            _currentTransaction = await _sqlServerDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to commit.");

            await _sqlServerDbContext.SaveChangesAsync();
            await _currentTransaction.CommitAsync();

            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task CreateSavepointAsync(string savepointName)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to create a savepoint.");

            await _currentTransaction.CreateSavepointAsync(savepointName);
        }

        public async Task RollbackToSavepointAsync(string savepointName)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to rollback to a savepoint.");

            await _currentTransaction.RollbackToSavepointAsync(savepointName);
        }

        public async Task ReleaseSavepointAsync(string savepointName)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to release a savepoint.");

            await _currentTransaction.ReleaseSavepointAsync(savepointName);
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}
