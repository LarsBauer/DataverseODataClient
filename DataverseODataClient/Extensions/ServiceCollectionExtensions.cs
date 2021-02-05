using System;
using DataverseODataClient.Auth;
using DataverseODataClient.Middlewares;
using DataverseODataClient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Simple.OData.Client;

namespace DataverseODataClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataverseODataClient(this IServiceCollection services,
            Action<DataverseODataClientOptions> options)
        {
            services.Configure(options);

            // token provider
            services.AddSingleton<ITokenProvider, DataverseTokenProvider>();
            // correlation id provider
            services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICorrelationIdProvider, HttpHeaderCorrelationIdProvider>();
            // Web API endpoint provider
            services.AddSingleton<IWebApiEndpointProvider, WebApiEndpointProvider>();

            // outgoing request middlewares
            services.AddTransient<AuthorizationHeaderHandler>();
            services.AddTransient<CorrelationIdHandler>();

            // configure HttpClient
            services.AddHttpClient<ODataClientSettings, ODataClientSettings>((serviceProvider, client) =>
                {
                    var endpointProvider = serviceProvider.GetRequiredService<IWebApiEndpointProvider>();

                    client.BaseAddress = endpointProvider.GetWebApiEndpoint();
                })
                .AddHttpMessageHandler<AuthorizationHeaderHandler>()
                .AddHttpMessageHandler<CorrelationIdHandler>();

            // register OData client
            services.AddScoped<IODataClient, DataverseODataClient>();

            return services;
        }
    }
}