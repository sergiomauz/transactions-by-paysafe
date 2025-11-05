using Application.Infrastructure.ExternalServices;
using ExternalServices.ApiTransactions;
using ExternalServices.KafkaClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Initialize all the External services
        /// </summary>
        /// <param name="services">Contract collection of service descriptor</param>
        /// <param name="configuration">App configuration</param>
        /// <returns>Contract collection of service descriptor</returns>
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Kafka Client
            var kafkaClientSection = configuration.GetSection("KafkaClient");
            services.Configure<KafkaClientSettings>(kafkaClientSection);
            services.AddSingleton<IKafkaClientProducerService, KafkaClientProducerService>();
            services.AddSingleton<IKafkaClientConsumerService, KafkaClientConsumerService>();

            // ApiTransactions Client
            var apiTransactionsSection = configuration.GetSection("ApiTransactions");
            services.Configure<ApiTransactionsSettings>(apiTransactionsSection);
            services.AddSingleton<IApiTransactionsService, ApiTransactionsService>();

            //
            return services;
        }
    }
}
