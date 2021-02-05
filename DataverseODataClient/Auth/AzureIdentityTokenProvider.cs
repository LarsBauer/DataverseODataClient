using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace DataverseODataClient.Auth
{
    internal abstract class AzureIdentityTokenProvider : ITokenProvider
    {
        private static readonly TimeSpan ExpirationThreshold = TimeSpan.FromMinutes(5);

        private readonly TokenCredential _tokenCredential;
        private readonly string[] _scopes;

        private AccessToken _accessToken;

        protected AzureIdentityTokenProvider(string[] scopes, TokenCredential tokenCredential)
        {
            _scopes = scopes;
            _tokenCredential = tokenCredential;
        }

        public async Task<AccessToken> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            if (IsAccessTokenExpired(_accessToken))
            {
                _accessToken = await _tokenCredential.GetTokenAsync(new TokenRequestContext(_scopes), cancellationToken)
                    .ConfigureAwait(false);
            }

            return _accessToken;
        }

        private static bool IsAccessTokenExpired(AccessToken accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken.Token)) return true;

            return DateTime.UtcNow + ExpirationThreshold >= accessToken.ExpiresOn;
        }
    }
}