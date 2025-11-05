using Application.Infrastructure.ExternalServices;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace ExternalServices.KafkaClient
{
    public class KafkaClientConsumerService : IKafkaClientConsumerService, IDisposable
    {
        private readonly KafkaClientSettings _settings;
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaClientConsumerService(IOptions<KafkaClientSettings> settings)
        {
            _settings = settings.Value;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = $"{_settings.RemoteHost}:{_settings.RemotePort}",
                GroupId = $"{_settings.ClientId}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        public string ConsumeAccountTransaction()
        {
            // Check out if topic exists, or exit
            using var adminClient = new AdminClientBuilder(
                new AdminClientConfig
                {
                    BootstrapServers = $"{_settings.RemoteHost}:{_settings.RemotePort}",
                }
            ).Build();
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(15));
            if (!metadata.Topics.Any(t => t.Topic == _settings.AccountTransactionsTopic))
            {
                return string.Empty;
            }

            // Subscribe to Kafka Server with a Topic and start to consume messages, or exit even if message is null
            _consumer.Subscribe(_settings.AccountTransactionsTopic);
            var consumeResult = _consumer.Consume();
            if (consumeResult?.Message == null)
            {
                return string.Empty;
            }

            // Read message and return            
            return consumeResult.Message.Value;
        }

        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }
    }
}
