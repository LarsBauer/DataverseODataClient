using System;

namespace BauerApps.DataverseODataClient
{
    /// <summary>
    /// Dataverse OData client configuration options
    /// </summary>
    public class DataverseODataClientOptions
    {
        /// <summary>
        /// The base url of your Dataverse environment in the following format:
        /// https://{{organizationName}}.crm4.dynamics.com/api/data/v9.1/
        /// </summary>
        public Uri OrganizationUrl { get; set; }

        /// <summary>
        /// When using a user-assigned managed identity in Azure you have to specifiy the client id
        /// </summary>
        public string ManagedIdentityClientId { get; set; }

        /// <summary>
        /// The name of the HTTP header which contains the correlation id
        /// </summary>
        public string CorrelationIdHeader { get; set; } = "X-Correlation-Id";
    }
}