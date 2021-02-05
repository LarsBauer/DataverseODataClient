using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataverseODataClient.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace DataverseODataClient.Middlewares
{
    internal class CorrelationIdHandler : DelegatingHandler
    {
        private const string CorrelationIdQueryParameter = "tag";

        private readonly ICorrelationIdProvider _provider;

        public CorrelationIdHandler(ICorrelationIdProvider provider = null)
        {
            _provider = provider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // skip execution when no provider is registered
            if (_provider == null) return base.SendAsync(request, cancellationToken);

            // extract correlation id from http headers
            var correlationId = _provider.GetCorrelationId();
            if (string.IsNullOrWhiteSpace(correlationId)) return base.SendAsync(request, cancellationToken);

            // add correlation id as query parameter
            var requestUrl = request.RequestUri?.AbsoluteUri ?? throw new InvalidOperationException();
            request.RequestUri =
                new Uri(QueryHelpers.AddQueryString(requestUrl, CorrelationIdQueryParameter, correlationId));

            return base.SendAsync(request, cancellationToken);
        }
    }
}