using System;

namespace BauerApps.DataverseODataClient.Services
{
    internal interface IWebApiEndpointProvider
    {
        Uri GetWebApiEndpoint();
    }
}