﻿using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace DataverseODataClient.Auth
{
    public interface ITokenProvider
    {
        Task<AccessToken> GetTokenAsync(CancellationToken cancellationToken = default);
    }
}