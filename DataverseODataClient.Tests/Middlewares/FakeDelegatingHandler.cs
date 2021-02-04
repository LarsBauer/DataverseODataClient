using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DataverseODataClient.Tests.Middlewares
{
    public class FakeDelegatingHandler : DelegatingHandler
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