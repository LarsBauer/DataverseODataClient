using System;
using DataverseODataClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DataverseODataClient.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ShouldDoSomething()
        {
            // Arrange
            var sut = new ServiceCollection();

            // Act
            sut.AddDataverseODataClient(options => { options.OrganizationUrl = new Uri(""); });

            // Assert
        }
    }
}