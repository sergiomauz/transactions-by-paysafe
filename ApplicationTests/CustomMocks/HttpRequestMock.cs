using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Bogus;
using Moq;

namespace Unit.Tests.CustomMocks
{
    public class HttpRequestMock
    {
        public Mock<HttpRequest> Create()
        {
            var faker = new Faker();
            var claims = new List<Claim>
            {
                new Claim("Username", faker.Random.AlphaNumeric(10))
            };

            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(x => x.User).Returns(principal);

            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.SetupGet(x => x.HttpContext).Returns(httpContextMock.Object);

            return httpRequestMock;
        }
    }
}
