using System;

namespace DataverseODataClient.Services
{
    internal interface IWebApiEndpointProvider
    {
        Uri GetWebApiEndpoint();
    }
}