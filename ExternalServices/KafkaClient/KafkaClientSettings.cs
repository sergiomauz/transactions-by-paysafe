namespace ExternalServices.KafkaClient
{
    public class KafkaClientSettings
    {
        public string RemoteHost { get; set; }
        public int RemotePort { get; set; }
        public int LingerMs { get; set; }
        public string ClientId { get; set; }
        public string AccountTransactionsTopic { get; set; }
        public string AccountTransactionsTopicFails { get; set; }
    }
}
