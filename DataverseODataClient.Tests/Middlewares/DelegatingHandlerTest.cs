using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DataverseODataClient.Tests.Middlewares
{
    public abstract class DelegatingHandlerTest
    {
        protected async Task<HttpRequestMessage> InvokeAsync(DelegatingHandler handler, HttpRequestMessage request)
        {
            var testHandler = new TestDelegatingHandler();
            handler.InnerHandler = testHandler;

            var invoker = new HttpMessageInvoker(handler);

            await invoker.SendAsync(request, CancellationToken.None);

            return testHandler.Request;
        }

        private class TestDelegatingHandler : DelegatingHandler
        {
            public HttpRequestMessage Request { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                Request = request;

                return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        }
    }
}