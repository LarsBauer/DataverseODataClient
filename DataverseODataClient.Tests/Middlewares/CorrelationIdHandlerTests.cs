using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataverseODataClient.Middlewares;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DataverseODataClient.Tests.Middlewares
{
    public class CorrelationIdHandlerTests
    {
        [Fact]
        public async Task ShouldAddCorrelationIdAsQueryParameterWhenPresentInHttpHeader()
        {
            // Arrange
            var headers = new HeaderDictionary
            {
                { "x-correlation-id", "myCorrelationId" }
            };

            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            A.CallTo(() => httpContextAccessor.HttpContext.Request.Headers)
                .Returns(headers);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            var fakeHandler = new FakeDelegatingHandler();

            var sut = new CorrelationIdHandler(httpContextAccessor)
            {
                InnerHandler = fakeHandler
            };

            var invoker = new HttpMessageInvoker(sut);

            // Act
            var _ = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            fakeHandler.Request.RequestUri?.Query.Should().Contain("tag=myCorrelationId");
        }

        [Fact]
        public async Task ShouldSkipExecutionWhenCorrelationIdIsNotPresentInHttpHeader()
        {
            // Arrange
            var headers = new HeaderDictionary();

            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            A.CallTo(() => httpContextAccessor.HttpContext.Request.Headers)
                .Returns(headers);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            var fakeHandler = new FakeDelegatingHandler();

            var sut = new CorrelationIdHandler(httpContextAccessor)
            {
                InnerHandler = fakeHandler
            };

            var invoker = new HttpMessageInvoker(sut);

            // Act
            var _ = await invoker.SendAsync(request, CancellationToken.None);

            // Assert
            fakeHandler.Request.RequestUri?.Query.Should().BeNullOrEmpty();
        }
    }
}