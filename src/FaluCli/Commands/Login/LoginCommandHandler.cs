using Falu.Config;
using IdentityModel;
using IdentityModel.Client;
using System.Diagnostics;

namespace Falu.Commands.Login;

internal class LoginCommandHandler : ICommandHandler
{
    private readonly HttpClient client;
    private readonly IConfigValuesProvider configValuesProvider;
    private readonly ILogger logger;
    private readonly IDiscoveryCache discoveryCache;

    public LoginCommandHandler(IHttpClientFactory httpClientFactory, IConfigValuesProvider configValuesProvider, ILogger<LoginCommandHandler> logger)
    {
        // unfortunately, injecting does not work, instead if gives the default client instance
        client = httpClientFactory?.CreateClient(nameof(LoginCommandHandler)) ?? throw new ArgumentNullException(nameof(httpClientFactory));
        this.configValuesProvider = configValuesProvider ?? throw new ArgumentNullException(nameof(configValuesProvider));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        discoveryCache = new DiscoveryCache(Constants.Authority, () => client);
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();

        // perform confirguration discovery
        var disco = await discoveryCache.GetAsync();
        if (disco.IsError) throw new LoginException(disco.Error, disco.Exception);

        // perform device authorization
        var auth_resp = await RequestAuthorizationAsync(disco, cancellationToken);

        // get the token via polling
        var token_resp = await RequestTokenAsync(disco, auth_resp, cancellationToken);
        logger.LogInformation("Authentication tokens issued successfully.");

        // save the authentication information
        var config = await configValuesProvider.GetConfigValuesAsync(cancellationToken);
        config.Authentication = new AuthenticationTokenConfigData
        {
            AccessToken = token_resp.AccessToken,
            RefreshToken = token_resp.RefreshToken,
            AccessTokenExpiry = DateTimeOffset.UtcNow.AddSeconds(token_resp.ExpiresIn).AddSeconds(-5),
        };
        await configValuesProvider.SetConfigValuesAsync(cancellationToken);
        logger.LogInformation("Authentication tokens issued successfully.");

        return 0;
    }

    private async Task<DeviceAuthorizationResponse> RequestAuthorizationAsync(DiscoveryDocumentResponse disco, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Performing device authentication. You will be redirected to the browser.");

        var request = new DeviceAuthorizationRequest
        {
            Address = disco.DeviceAuthorizationEndpoint,
            ClientId = Constants.ClientId,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            Scope = Constants.Scopes,
        };
        var response = await client.RequestDeviceAuthorizationAsync(request, cancellationToken);
        if (response.IsError) throw new LoginException(response.Error);

        logger.LogInformation("Complete authentication in the browser using the following information:");
        logger.LogInformation("User code   : {UserCode}", response.UserCode);
        logger.LogInformation("Device code : {DeviceCode}", response.DeviceCode);
        logger.LogInformation("Opening your browser at {VerificationUri}", response.VerificationUri);

        // delay for 2 seconds before opening the browser for the user to see the code
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        Process.Start(new ProcessStartInfo(response.VerificationUriComplete) { UseShellExecute = true });

        return response;
    }

    private async Task<TokenResponse> RequestTokenAsync(DiscoveryDocumentResponse disco, DeviceAuthorizationResponse auth, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var request = new DeviceTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = Constants.ClientId,
                DeviceCode = auth.DeviceCode,
            };
            var response = await client.RequestDeviceTokenAsync(request, cancellationToken);

            if (response.IsError)
            {
                if (response.Error == OidcConstants.TokenErrors.AuthorizationPending || response.Error == OidcConstants.TokenErrors.SlowDown)
                {
                    logger.LogInformation("{Error}... waiting.", response.Error);
                    await Task.Delay(TimeSpan.FromSeconds(auth.Interval), cancellationToken);
                }
                else
                {
                    throw new LoginException(response.Error, response.Exception);
                }
            }
            else
            {
                return response;
            }
        }
    }
}
