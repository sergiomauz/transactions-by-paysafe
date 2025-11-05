namespace Application.Infrastructure.ExternalServices
{
    public interface IKafkaClientProducerService
    {
        Task<bool> PublishAccountTransactionAsync(string transactionId, string sourceAccountId, string transactionDate, decimal accumulatedValue, decimal currentValue);
        Task<bool> PublishAccountTransactionFailAsync(string transactionId, string sourceAccountId, string transactionDate, decimal accumulatedValue, decimal currentValue);
    }
}
