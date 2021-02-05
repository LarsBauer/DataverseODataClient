using System;
using System.Net.Http;
using DataverseODataClient.Auth;
using DataverseODataClient.Extensions;
using DataverseODataClient.Middlewares;
using DataverseODataClient.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Simple.OData.Client;
using Xunit;

namespace DataverseODataClient.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ShouldRegisterAllRequiredService()
        {
            // Arrange
            var sut = new ServiceCollection();

            // Act
            sut.AddDataverseODataClient(options => { options.OrganizationUrl = new Uri("http://localhost"); });

            // Assert
            sut.Should().Contain(x =>
                x.ServiceType == typeof(ITokenProvider) && x.ImplementationType == typeof(DataverseTokenProvider) &&
                x.Lifetime == ServiceLifetime.Singleton);
            sut.Should().Contain(x =>
                x.ServiceType == typeof(ICorrelationIdProvider) &&
                x.ImplementationType == typeof(HttpHeaderCorrelationIdProvider));
            sut.Should().Contain(x =>
                x.ServiceType == typeof(IWebApiEndpointProvider) &&
                x.ImplementationType == typeof(WebApiEndpointProvider));
            sut.Should().Contain(x => x.ServiceType == typeof(AuthorizationHeaderHandler));
            sut.Should().Contain(x => x.ServiceType == typeof(CorrelationIdHandler));
            sut.Should().Contain(x => x.ServiceType == typeof(ODataClientSettings));
            sut.Should().Contain(x => x.ServiceType == typeof(IHttpClientFactory));
            sut.Should().Contain(x =>
                x.ServiceType == typeof(IODataClient) && x.ImplementationType == typeof(DataverseODataClient));
        }

        [Fact]
        public void ShouldConfigureOptions()
        {
            // Arrange
            var organizationUrl = new Uri("https://my-organization.crm4.dynamics.com");
            const string clientId = "myUserAssignedManagedIdentityClientId";

            var sut = new ServiceCollection();

            // Act
            sut.AddDataverseODataClient(o =>
            {
                o.OrganizationUrl = organizationUrl;
                o.ManagedIdentityClientId = clientId;
            });

            // Assert
            var provider = sut.BuildServiceProvider();

            var options = provider.GetRequiredService<IOptions<DataverseODataClientOptions>>();
            options.Value.OrganizationUrl.Should().Be(organizationUrl);
            options.Value.ManagedIdentityClientId.Should().Be(clientId);
        }

        [Fact]
        public void ShouldConfigureHttpClient()
        {
            // Arrange
            var sut = new ServiceCollection();

            // Act
            sut.AddDataverseODataClient(options =>
            {
                options.OrganizationUrl = new Uri("https://my-organization.crm4.dynamics.com/");
            });

            // Assert
            var provider = sut.BuildServiceProvider();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(ODataClientSettings));

            httpClient.BaseAddress.Should().Be("https://my-organization.crm4.dynamics.com/api/data/v9.1/");
        }
    }
}