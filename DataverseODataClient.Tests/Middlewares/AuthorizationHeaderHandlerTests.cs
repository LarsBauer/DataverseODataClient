using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using DataverseODataClient.Auth;
using DataverseODataClient.Middlewares;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DataverseODataClient.Tests.Middlewares
{
    public class AuthorizationHeaderHandlerTests
    {
        [Fact]
        public async Task ShouldAddBearerTokenToAuthorizationHeader()
        {
            // Arrange
            var accessToken = new AccessToken("MySecretAccessToken", DateTimeOffset.MaxValue);

            var tokenProvider = A.Fake<ITokenProvider>();
            A.CallTo(() => tokenProvider.GetTokenAsync(A<CancellationToken>._))
                .Returns(accessToken);

            var fakeHandler = new FakeDelegatingHandler();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");

            var sut = new AuthorizationHeaderHandler(tokenProvider)
            {
                InnerHandler = fakeHandler
            };

            var invoker = new HttpMessageInvoker(sut);

            // Act
            var _ = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            fakeHandler.Request.Headers.Authorization.Should().NotBeNull();
            fakeHandler.Request.Headers.Authorization?.Scheme.Should().Be("Bearer");
            fakeHandler.Request.Headers.Authorization?.Parameter.Should().Be(accessToken.Token);
        }
    }
}