using IdentityModel.Client;

namespace Falu.Config;

internal record ConfigValues
{
    public string? ActiveWorkspaceId { get; set; }
    public bool ActiveLiveMode { get; set; }
    public AuthenticationTokenConfigData? Authentication { get; set; }

    public void Update(TokenResponse response)
    {
        Authentication = new AuthenticationTokenConfigData
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            AccessTokenExpiry = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn).AddSeconds(-5),
        };
    }
}

internal record AuthenticationTokenConfigData
{
    public string? AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiry { get; set; }
    public string? RefreshToken { get; set; }

    public bool HasValidAccessToken() => !string.IsNullOrWhiteSpace(AccessToken) && AccessTokenExpiry > DateTimeOffset.UtcNow;
    public bool HasValidRefreshToken() => !string.IsNullOrWhiteSpace(RefreshToken);
}
