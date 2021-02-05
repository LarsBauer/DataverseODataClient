using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using DataverseODataClient.Auth;
using DataverseODataClient.Extensions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DataverseODataClient.Tests.Auth
{
    public class DataverseTokenProviderTests
    {
        [Fact]
        public async Task ShouldReturnAcessToken()
        {
            // Arrange
            var options = Options.Create(new DataverseODataClientOptions
            {
                OrganizationUrl = new Uri("https://my-organization.crm4.dynamics.com")
            });

            var accessToken = new AccessToken("123", DateTimeOffset.MaxValue);
            var credential = A.Fake<TokenCredential>();
            A.CallTo(() => credential.GetTokenAsync(A<TokenRequestContext>._, A<CancellationToken>._))
                .Returns(accessToken);

            var sut = new DataverseTokenProvider(options, credential);
            // Act
            var token = await sut.GetTokenAsync();

            // Assert
            token.Should().Be(accessToken);
        }

        [Fact]
        public async Task ShouldCacheAccessTokenWhenRequestedMultipleTimes()
        {
            // Arrange
            var options = Options.Create(new DataverseODataClientOptions
            {
                OrganizationUrl = new Uri("https://my-organization.crm4.dynamics.com")
            });

            var accessToken = new AccessToken("123", DateTimeOffset.MaxValue);
            var credential = A.Fake<TokenCredential>();
            A.CallTo(() => credential.GetTokenAsync(A<TokenRequestContext>._, A<CancellationToken>._))
                .Returns(accessToken);

            var sut = new DataverseTokenProvider(options, credential);

            // Act
            await sut.GetTokenAsync();
            await sut.GetTokenAsync();
            await sut.GetTokenAsync();

            // Assert
            A.CallTo(() => credential.GetTokenAsync(A<TokenRequestContext>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldRefreshAccessTokenWhenAccessTokenIsExpired()
        {
            // Arrange
            var options = Options.Create(new DataverseODataClientOptions
            {
                OrganizationUrl = new Uri("https://my-organization.crm4.dynamics.com")
            });

            var oldToken = new AccessToken("123", DateTimeOffset.UtcNow);
            var newToken = new AccessToken("123", DateTimeOffset.MaxValue);
            var credential = A.Fake<TokenCredential>();
            A.CallTo(() => credential.GetTokenAsync(A<TokenRequestContext>._, A<CancellationToken>._))
                .Returns(oldToken)
                .Once()
                .Then
                .Returns(newToken);

            var sut = new DataverseTokenProvider(options, credential);

            // Act
            await sut.GetTokenAsync();
            var accessToken = await sut.GetTokenAsync();

            // Assert
            accessToken.Should().Be(newToken);
        }
    }
}