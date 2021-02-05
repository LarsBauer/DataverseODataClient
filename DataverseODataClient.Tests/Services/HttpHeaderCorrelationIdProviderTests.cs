using System.Collections.Generic;
using BauerApps.DataverseODataClient.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace BauerApps.DataverseODataClient.Tests.Services
{
    public class HttpHeaderCorrelationIdProviderTests
    {
        [Fact]
        public void ShouldExtractCorrelationIdFromHttpHeader()
        {
            // Arrange
            const string correlationId = "myCorrelationId";
            var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "x-correlation-id", correlationId }
            });

            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            A.CallTo(() => httpContextAccessor.HttpContext.Request.Headers)
                .Returns(headers);

            var sut = new HttpHeaderCorrelationIdProvider(httpContextAccessor);

            // Act
            var result = sut.GetCorrelationId();

            // Assert
            result.Should().Be(correlationId);
        }

        [Fact]
        public void ShouldReturnNullWhenHttpHeaderIsNotPresent()
        {
            // Arrange
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            A.CallTo(() => httpContextAccessor.HttpContext.Request.Headers)
                .Returns(new HeaderDictionary());

            var sut = new HttpHeaderCorrelationIdProvider(httpContextAccessor);

            // Act
            var result = sut.GetCorrelationId();

            // Assert
            result.Should().BeNull();
        }
    }
}