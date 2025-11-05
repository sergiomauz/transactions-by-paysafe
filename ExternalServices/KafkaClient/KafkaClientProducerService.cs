using System.Text.Json;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Application.Infrastructure.ExternalServices;

namespace ExternalServices.KafkaClient
{
    public class KafkaClientProducerService : IKafkaClientProducerService, IDisposable
    {
        private readonly KafkaClientSettings _settings;
        private readonly IProducer<Null, string> _producer;

        public KafkaClientProducerService(IOptions<KafkaClientSettings> settings)
        {
            _settings = settings.Value;
            var config = new ProducerConfig
            {
                BootstrapServers = $"{_settings.RemoteHost}:{_settings.RemotePort}",
                Acks = Acks.All,
                EnableIdempotence = true,
                MessageSendMaxRetries = 3,
                LingerMs = _settings.LingerMs,
                BatchSize = 100_000,
                CompressionType = CompressionType.Snappy
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task<bool> PublishAccountTransactionAsync(string transactionId, string sourceAccountId, string transactionDate, decimal accumulatedValue, decimal currentValue)
        {
            var kafkaMessage = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(
                    new
                    {
                        TransactionId = transactionId,
                        SourceAccountId = sourceAccountId,
                        TransactionDate = transactionDate,
                        AccumulatedValue = accumulatedValue,
                        CurrentValue = currentValue
                    })
            };
            var result = await _producer.ProduceAsync(_settings.AccountTransactionsTopic, kafkaMessage);
            if (result.Status == PersistenceStatus.Persisted)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PublishAccountTransactionFailAsync(string transactionId, string sourceAccountId, string transactionDate, decimal accumulatedValue, decimal currentValue)
        {
            var kafkaMessage = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(
                    new
                    {
                        TransactionId = transactionId,
                        SourceAccountId = sourceAccountId,
                        TransactionDate = transactionDate,
                        AccumulatedValue = accumulatedValue,
                        CurrentValue = currentValue
                    })
            };
            var result = await _producer.ProduceAsync(_settings.AccountTransactionsTopicFails, kafkaMessage);
            if (result.Status == PersistenceStatus.Persisted)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}
