using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Extensions.Caching.Memory;

namespace BauerApps.DataverseODataClient.Auth
{
    internal abstract class AzureIdentityTokenProvider : ITokenProvider
    {
        private static readonly TimeSpan ExpirationThreshold = TimeSpan.FromMinutes(5);

        private readonly TokenCredential _tokenCredential;
        private readonly IMemoryCache _cache;
        private readonly string[] _scopes;

        protected AzureIdentityTokenProvider(string[] scopes, TokenCredential tokenCredential, IMemoryCache cache)
        {
            _scopes = scopes;
            _tokenCredential = tokenCredential;
            _cache = cache;
        }

        public async Task<AccessToken> GetTokenAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(_scopes);

            if (!_cache.TryGetValue<AccessToken>(cacheKey, out var accessToken))
            {
                accessToken = await _tokenCredential.GetTokenAsync(new TokenRequestContext(_scopes), cancellationToken)
                    .ConfigureAwait(false);

                _cache.Set(cacheKey, accessToken, accessToken.ExpiresOn - ExpirationThreshold);
            }

            return accessToken;
        }

        private static string GetCacheKey(IEnumerable<string> scopes)
        {
            return string.Join('|', scopes);
        }
    }
}