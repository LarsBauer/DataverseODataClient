using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using DataverseODataClient.Auth;

namespace DataverseODataClient.Middlewares
{
    internal class AuthorizationHeaderHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthorizationHeaderHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // acquire access token and add authorization header
            var accessToken = await _tokenProvider.GetTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}