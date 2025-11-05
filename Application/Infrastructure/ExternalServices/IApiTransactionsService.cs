namespace Application.Infrastructure.ExternalServices
{
    public interface IApiTransactionsService
    {
        Task<int> UpdateAccountTransactionAsync(string id, int accountTransactionStatus);
    }
}
