using System;
using DataverseODataClient.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Simple.OData.Client;
using Xunit;

namespace DataverseODataClient.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        // [Fact]
        public void ShouldDoSomething()
        {
            // Arrange
            var sut = new ServiceCollection();
            sut.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            // Act
            sut.AddDataverseODataClient(options => { options.OrganizationUrl = new Uri("http://localhost"); });
            var provider = sut.BuildServiceProvider();

            var settings = provider.GetService<ODataClientSettings>();

            // Assert
            settings.HttpClient.BaseAddress.Should().Be("http://localhost/api/data/v9.1/");
        }
    }
}