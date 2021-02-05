using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace DataverseODataClient.Auth
{
    internal class DataverseTokenProvider : AzureIdentityTokenProvider
    {
        private const string DefaultScope = "/.default";

        public DataverseTokenProvider(IConfiguration configuration, TokenCredential tokenCredential = null)
            : base(ConfigureScopes(configuration), tokenCredential ?? ConfigureTokenCredential(configuration))
        {
        }

        private static string[] ConfigureScopes(IConfiguration configuration)
        {
            var organizationUrl = configuration.GetValue<Uri>("OrganizationUrl");

            // required token scope for Dataverse Web API is https://<organizationName>.crm.dynamics.com/.default
            var baseUrl = new Uri(organizationUrl.GetLeftPart(UriPartial.Authority));
            var scope = new Uri(baseUrl, DefaultScope).AbsoluteUri;

            return new[] { scope };
        }

        private static TokenCredential ConfigureTokenCredential(IConfiguration configuration)
        {
            var userAssignedClientId = configuration.GetValue<string>("ManagedIdentityClientId");
            // we have to specify client id when using user-assigned managed identities
            return string.IsNullOrWhiteSpace(userAssignedClientId)
                ? new DefaultAzureCredential()
                : new DefaultAzureCredential(new DefaultAzureCredentialOptions
                    { ManagedIdentityClientId = userAssignedClientId });
        }
    }
}