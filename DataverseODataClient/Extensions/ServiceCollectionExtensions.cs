using System;
using BauerApps.DataverseODataClient.Auth;
using BauerApps.DataverseODataClient.Middlewares;
using BauerApps.DataverseODataClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Simple.OData.Client;

namespace BauerApps.DataverseODataClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataverseODataClient(this IServiceCollection services,
            Action<DataverseODataClientOptions> options)
        {
            services.Configure(options);

            // token provider
            services.AddMemoryCache();
            services.AddScoped<ITokenProvider, DataverseTokenProvider>();
            // correlation id provider
            services.AddHttpContextAccessor();
            services.AddTransient<ICorrelationIdProvider, HttpHeaderCorrelationIdProvider>();
            // Web API endpoint provider
            services.AddTransient<IWebApiEndpointProvider, WebApiEndpointProvider>();

            // outgoing request middlewares
            services.AddTransient<AuthorizationHeaderHandler>();
            services.AddTransient<CorrelationIdHandler>();

            // register OData client with preconfigured HttpClient
            services.AddHttpClient<IODataClient, DataverseODataClient>((serviceProvider, client) =>
                {
                    var endpointProvider = serviceProvider.GetRequiredService<IWebApiEndpointProvider>();

                    client.BaseAddress = endpointProvider.GetWebApiEndpoint();
                })
                .AddHttpMessageHandler<AuthorizationHeaderHandler>()
                .AddHttpMessageHandler<CorrelationIdHandler>();

            return services;
        }
    }
}