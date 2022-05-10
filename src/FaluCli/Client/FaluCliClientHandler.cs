using Falu.Config;
using IdentityModel.Client;
using System.Net.Http.Headers;
using Res = Falu.Properties.Resources;

namespace Falu.Client;

internal class FaluCliClientHandler : DelegatingHandler
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IDiscoveryCache discoveryCache;
    private readonly InvocationContext context;
    private readonly IConfigValuesProvider configValuesProvider;
    private readonly ILogger logger;

    public FaluCliClientHandler(IHttpClientFactory httpClientFactory,
                                IDiscoveryCache discoveryCache,
                                InvocationContext context,
                                IConfigValuesProvider configValuesProvider,
                                ILoggerFactory loggerFactory)
    {
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        this.discoveryCache = discoveryCache ?? throw new ArgumentNullException(nameof(discoveryCache));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.configValuesProvider = configValuesProvider ?? throw new ArgumentNullException(nameof(configValuesProvider));
        logger = loggerFactory?.CreateOpenIdLogger() ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var workspaceId = context.ParseResult.ValueForOption<string>("--workspace");
        var idempotencyKey = context.ParseResult.ValueForOption<string>("--idempotency-key");
        var live = context.ParseResult.ValueForOption<bool?>("--live");

        if (!string.IsNullOrWhiteSpace(workspaceId))
        {
            request.Headers.Add("X-Workspace-Id", workspaceId);
        }

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            request.Headers.Remove("X-Idempotency-Key"); // avoid multiple values or headers
            request.Headers.Add("X-Idempotency-Key", idempotencyKey);
        }

        if (live is not null)
        {
            request.Headers.Remove("X-Live-Mode"); // avoid multiple values or headers
            request.Headers.Add("X-Live-Mode", live.Value.ToString().ToLowerInvariant());
        }

        var key = context.ParseResult.ValueForOption<string>("--apikey");

        // if we do not have a key, we need to have it pulled from the configuration
        if (string.IsNullOrWhiteSpace(key))
        {
            var config = await configValuesProvider.GetConfigValuesAsync(cancellationToken);

            // ensure we have login information and that it contains a valid access token or refresh token
            if (config.Authentication is null || (!config.Authentication.HasValidAccessToken() && !config.Authentication.HasValidRefreshToken()))
            {
                throw new FaluException(Res.AuthenticationInformationMissing);
            }

            // at this point, we either have a valid access token or an invalid acess token with a valid refresh token
            // if the access token is invalid, we need to get one via the refresh token
            if (!config.Authentication.HasValidAccessToken())
            {
                logger.LogInformation("Requesting for a new access token using the saved refresh token");
                // perform confirguration discovery
                var disco = await discoveryCache.GetSafelyAsync(cancellationToken);

                // request for a new token using the refresh token
                var rtr = new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = Constants.ClientId,
                    RefreshToken = config.Authentication.RefreshToken,
                };
                var client = httpClientFactory.CreateOpenIdClient();
                var token_resp = await client.RequestRefreshTokenAsync(rtr, cancellationToken);
                if (token_resp.IsError)
                {
                    throw new FaluException(Res.RefreshingAccessTokenFailed);
                }

                logger.LogInformation("Access token refreshed.");
                await configValuesProvider.SaveConfigValuesAsync(token_resp, cancellationToken);
            }

            key = config.Authentication.AccessToken;
        }

        // at this point we have a key and we can proceed to set the authentication header
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

        // perform the request
        return await base.SendAsync(request, cancellationToken);
    }
}
