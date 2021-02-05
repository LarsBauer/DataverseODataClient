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
    public class AuthorizationHeaderHandlerTests : DelegatingHandlerTest
    {
        [Fact]
        public async Task ShouldAddBearerTokenToAuthorizationHeader()
        {
            // Arrange
            var accessToken = new AccessToken("MySecretAccessToken", DateTimeOffset.MaxValue);

            var tokenProvider = A.Fake<ITokenProvider>();
            A.CallTo(() => tokenProvider.GetTokenAsync(A<CancellationToken>._))
                .Returns(accessToken);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");

            var sut = new AuthorizationHeaderHandler(tokenProvider);

            // Act
            var result = await InvokeAsync(sut, request);

            // Assert
            result.Headers.Authorization.Should().NotBeNull();
            result.Headers.Authorization?.Scheme.Should().Be("Bearer");
            result.Headers.Authorization?.Parameter.Should().Be(accessToken.Token);
        }
    }
}