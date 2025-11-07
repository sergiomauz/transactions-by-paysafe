using System.Net;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediatR;

namespace Application.Auth.ApiKeyProtection
{
    public class ApiKeyProtectionHandler :
        IRequestHandler<ApiKeyProtectionCommand, HttpStatusCode>
    {
        private readonly ApplicationSettings _settings;
        private readonly ILogger<ApiKeyProtectionHandler> _logger;

        public ApiKeyProtectionHandler(
            IOptions<ApplicationSettings> settings,
            ILogger<ApiKeyProtectionHandler> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        private static string _computeHmac(string key, string data)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

            return Convert.ToHexString(hash).ToLower();
        }

        private static bool _areEqual(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);

            return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
        }

        public async Task<HttpStatusCode> Handle(ApiKeyProtectionCommand command, CancellationToken cancellationToken)
        {
            var apiKey = command.Request.Headers["X-API-KEY"].FirstOrDefault();
            var signature = command.Request.Headers["X-SIGNATURE"].FirstOrDefault();

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(signature))
            {
                _logger.LogWarning($"Request unauthorized. Api-Key and Signature required.");

                return HttpStatusCode.Unauthorized;
            }

            var client = _settings.Clients.FirstOrDefault(c => c.ApiKey == apiKey);
            if (client == null)
            {
                _logger.LogWarning($"Request unauthorized. Api-Key not found.");

                return HttpStatusCode.Unauthorized;
            }

            // Optional: If you requires extra security, can enable this code for using signatures and HMAC
            #region EXTRA-SECURITY
            //try
            //{
            //    command.Request.EnableBuffering();
            //    using var reader = new StreamReader(command.Request.Body, Encoding.UTF8, leaveOpen: true);
            //    var body = await reader.ReadToEndAsync();
            //    command.Request.Body.Position = 0;

            //    var computedSignature = _computeHmac(client.PrivateKey, body);
            //    if (!_areEqual(signature, computedSignature))
            //    {
            //        _logger.LogWarning($"Invalid signature for client {client.ApiKey}.");

            //        return HttpStatusCode.Unauthorized;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error validating HMAC signature.");

            //    return HttpStatusCode.InternalServerError;
            //}
            #endregion

            return HttpStatusCode.OK;
        }
    }
}
