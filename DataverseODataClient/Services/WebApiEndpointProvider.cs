using System;
using Microsoft.Extensions.Options;

namespace BauerApps.DataverseODataClient.Services
{
    internal class WebApiEndpointProvider : IWebApiEndpointProvider
    {
        private const string WebApiPath = "/api/data/v9.1/";

        private readonly DataverseODataClientOptions _options;

        public WebApiEndpointProvider(IOptions<DataverseODataClientOptions> options)
        {
            _options = options.Value;
        }

        public Uri GetWebApiEndpoint()
        {
            var organizationUrl = _options.OrganizationUrl;

            return organizationUrl.LocalPath == WebApiPath
                ? organizationUrl
                : new Uri(organizationUrl, WebApiPath);
        }
    }
}