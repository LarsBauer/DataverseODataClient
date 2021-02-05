using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;

namespace BauerApps.DataverseODataClient.Auth
{
    internal class DataverseTokenProvider : AzureIdentityTokenProvider
    {
        private const string DefaultScope = "/.default";

        public DataverseTokenProvider(IOptions<DataverseODataClientOptions> options,
            TokenCredential tokenCredential = null)
            : base(ConfigureScopes(options.Value), tokenCredential ?? ConfigureTokenCredential(options.Value))
        {
        }

        private static string[] ConfigureScopes(DataverseODataClientOptions options)
        {
            // required token scope for Dataverse Web API is https://<organizationName>.crm.dynamics.com/.default
            var baseUrl = new Uri(options.OrganizationUrl.GetLeftPart(UriPartial.Authority));
            var scope = new Uri(baseUrl, DefaultScope).AbsoluteUri;

            return new[] { scope };
        }

        private static TokenCredential ConfigureTokenCredential(DataverseODataClientOptions options)
        {
            var userAssignedClientId = options.ManagedIdentityClientId;
            // we have to specify client id when using user-assigned managed identities
            return string.IsNullOrWhiteSpace(userAssignedClientId)
                ? new DefaultAzureCredential()
                : new DefaultAzureCredential(new DefaultAzureCredentialOptions
                    { ManagedIdentityClientId = userAssignedClientId });
        }
    }
}