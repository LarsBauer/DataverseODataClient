using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BauerApps.DataverseODataClient.Services
{
    internal class HttpHeaderCorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _correlationIdHeader;

        public HttpHeaderCorrelationIdProvider(IHttpContextAccessor httpContextAccessor, IOptions<DataverseODataClientOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _correlationIdHeader = options.Value.CorrelationIdHeader;
        }

        public string GetCorrelationId()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers[_correlationIdHeader];
        }
    }
}