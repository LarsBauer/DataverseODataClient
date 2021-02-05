using Microsoft.AspNetCore.Http;

namespace DataverseODataClient.Services
{
    internal class HttpHeaderCorrelationIdProvider : ICorrelationIdProvider
    {
        private const string CorrelationIdHeader = "x-correlation-id";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpHeaderCorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCorrelationId()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers[CorrelationIdHeader];
        }
    }
}