using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace DataverseODataClient.Middlewares
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private const string CorrelationIdHeader = "x-correlation-id";
        private const string CorrelationIdQueryParameter = "tag";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // extract correlation id from http headers
            var correlationId = _httpContextAccessor.HttpContext?.Request.Headers[CorrelationIdHeader];
            if (string.IsNullOrWhiteSpace(correlationId)) return base.SendAsync(request, cancellationToken);

            // add correlation id as query parameter
            var requestUrl = request.RequestUri?.AbsoluteUri ?? throw new InvalidOperationException();
            request.RequestUri =
                new Uri(QueryHelpers.AddQueryString(requestUrl, CorrelationIdQueryParameter, correlationId));

            return base.SendAsync(request, cancellationToken);
        }
    }
}