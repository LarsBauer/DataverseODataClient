using System.Net.Http;
using System.Threading.Tasks;
using BauerApps.DataverseODataClient.Middlewares;
using BauerApps.DataverseODataClient.Services;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace BauerApps.DataverseODataClient.Tests.Middlewares
{
    public class CorrelationIdHandlerTests : DelegatingHandlerTest
    {
        [Fact]
        public async Task ShouldAddCorrelationIdAsQueryParameterWhenProvided()
        {
            // Arrange
            const string correlationId = "myCorrelationId";
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            A.CallTo(() => correlationIdProvider.GetCorrelationId())
                .Returns(correlationId);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");

            var sut = new CorrelationIdHandler(correlationIdProvider);

            // Act
            var result = await InvokeAsync(sut, request);

            // Assert
            result.RequestUri?.Query.Should().Contain($"tag={correlationId}");
        }

        [Fact]
        public async Task ShouldSkipExecutionWhenCorrelationIdIsNotProvided()
        {
            // Arrange
            var correlationIdProvider = A.Fake<ICorrelationIdProvider>();
            A.CallTo(() => correlationIdProvider.GetCorrelationId())
                .Returns(null);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");

            var sut = new CorrelationIdHandler(correlationIdProvider);

            // Act
            var result = await InvokeAsync(sut, request);

            // Assert
            result.RequestUri?.Query.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldSkipExecutionWhenNoProviderIsRegistered()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");

            var sut = new CorrelationIdHandler();

            // Act
            var result = await InvokeAsync(sut, request);

            // Assert
            result.Should().BeEquivalentTo(request);
        }
    }
}