using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RestSharp;
using Application.Infrastructure.ExternalServices;


namespace ExternalServices.ApiTransactions
{
    public class ApiTransactionsService : IApiTransactionsService
    {
        private readonly ApiTransactionsSettings _apiTransactionsSettings;
        private readonly ILogger<ApiTransactionsService> _logger;

        public ApiTransactionsService(
            IOptions<ApiTransactionsSettings> apiTransactionsSettings,
            ILogger<ApiTransactionsService> logger)
        {
            _apiTransactionsSettings = apiTransactionsSettings.Value;
            _logger = logger;
        }

        public async Task<int> UpdateAccountTransactionAsync(string id, int accountTransactionStatus)
        {
            var options = new RestClientOptions($"{_apiTransactionsSettings.RemoteBaseUrl}:{_apiTransactionsSettings.RemotePort}")
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var client = new RestClient(options);
            var url = string.Format(_apiTransactionsSettings.Resources.UpdateTransaction, id);
            var body = new
            {
                accountTransactionStatus
            };
            var request = new RestRequest(url, Method.Put);
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_apiTransactionsSettings.PrivateKey));
            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(body.ToString())));

            request.AddHeader("X-API-KEY", _apiTransactionsSettings.PublicKey);
            request.AddHeader("X-SIGNATURE", _apiTransactionsSettings.PrivateKey);
            request.Timeout = TimeSpan.FromSeconds(_apiTransactionsSettings.Timeout);
            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                _logger.LogCritical($"There were problems with the transactions API. Transaction ID: '{id}'");

                return (int)HttpStatusCode.BadGateway;
            }

            return (int)response.StatusCode;
        }
    }
}
