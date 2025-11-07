using System.Net;
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

        public async Task<HttpStatusCode> Handle(ApiKeyProtectionCommand command, CancellationToken cancellationToken)
        {
            var publicKey = command.Request.Headers["X-API-KEY"].FirstOrDefault();
            var signature = command.Request.Headers["X-SIGNATURE"].FirstOrDefault();

            if (publicKey == null || signature == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            return HttpStatusCode.OK;
        }
    }
}
