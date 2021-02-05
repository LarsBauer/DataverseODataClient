using System;
using DataverseODataClient.Auth;
using DataverseODataClient.Middlewares;
using DataverseODataClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Simple.OData.Client;

namespace DataverseODataClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string WebApiPath = "/api/data/v9.1/";

        public static IServiceCollection AddDataverseODataClient(this IServiceCollection services,
            Action<DataverseODataClientOptions> options)
        {
            services.Configure(options);

            // token provider
            services.AddSingleton<ITokenProvider, DataverseTokenProvider>();

            // correlation id provider
            services.AddScoped<ICorrelationIdProvider, HttpHeaderCorrelationIdProvider>();

            // outgoing request middlewares
            services.AddTransient<AuthorizationHeaderHandler>();
            services.AddTransient<CorrelationIdHandler>();

            // configure HttpClient
            services.AddHttpClient<ODataClientSettings, ODataClientSettings>((provider, client) =>
                {
                    client.BaseAddress = GetWebApiEndpoint(provider);
                })
                .AddHttpMessageHandler<AuthorizationHeaderHandler>()
                .AddHttpMessageHandler<CorrelationIdHandler>();

            // register OData client
            services.AddScoped<IODataClient, DataverseODataClient>();

            return services;
        }

        private static Uri GetWebApiEndpoint(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<DataverseODataClientOptions>>();
            var organizationUrl = options.Value.OrganizationUrl;

            return organizationUrl.LocalPath == WebApiPath
                ? organizationUrl
                : new Uri(organizationUrl, WebApiPath);
        }
    }
}