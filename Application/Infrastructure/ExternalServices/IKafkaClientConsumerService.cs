namespace Application.Infrastructure.ExternalServices
{
    public interface IKafkaClientConsumerService
    {
        string ConsumeAccountTransaction();
    }
}
