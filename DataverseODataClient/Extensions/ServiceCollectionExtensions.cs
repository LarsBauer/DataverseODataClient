using System;
using DataverseODataClient.Auth;
using DataverseODataClient.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simple.OData.Client;

namespace DataverseODataClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string WebApiPath = "/api/data/v9.1/";

        public static IServiceCollection AddODataClient(this IServiceCollection services, IConfiguration configuration)
        {
            // token provider
            services.AddSingleton<ITokenProvider, DataverseTokenProvider>();

            // outgoing request middlewares
            services.AddTransient<AuthorizationHeaderHandler>();
            services.AddTransient<CorrelationIdHandler>();

            // configure HttpClient
            services.AddHttpClient<ODataClientSettings, ODataClientSettings>(client =>
                {
                    client.BaseAddress = GetWebApiEndpoint(configuration);
                })
                .AddHttpMessageHandler<AuthorizationHeaderHandler>()
                .AddHttpMessageHandler<CorrelationIdHandler>();
            
            // register OData client
            services.AddScoped<IODataClient, DataverseODataClient>();

            return services;
        }

        private static Uri GetWebApiEndpoint(IConfiguration configuration)
        {
            var organizationUrl = configuration.GetValue<Uri>("OrganizationUrl");

            return organizationUrl.LocalPath == WebApiPath
                ? organizationUrl
                : new Uri(organizationUrl, WebApiPath);
        }
    }
}