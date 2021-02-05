using System;
using DataverseODataClient.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DataverseODataClient.Tests.Services
{
    public class WebApiEndpointProviderTests
    {
        [Theory]
        [InlineData("https://my-organization.crm4.dynamics.com")]
        [InlineData("https://my-organization.crm4.dynamics.com/")]
        [InlineData("https://my-organization.crm4.dynamics.com/api/data/v9.1")]
        [InlineData("https://my-organization.crm4.dynamics.com/api/data/v9.1/")]
        public void ShouldReturnWebApiEndpoint(string organizationUrl)
        {
            // Arrange
            var options = Options.Create(new DataverseODataClientOptions
            {
                OrganizationUrl = new Uri(organizationUrl)
            });

            var sut = new WebApiEndpointProvider(options);

            // Act
            var result = sut.GetWebApiEndpoint();

            // Assert
            result.Should().Be("https://my-organization.crm4.dynamics.com/api/data/v9.1/");
        }
    }
}