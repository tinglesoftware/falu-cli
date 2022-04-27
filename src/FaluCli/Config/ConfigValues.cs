namespace Falu.Config;

internal record ConfigValues
{
    public string? ActiveWorkspaceId { get; set; }
    public bool ActiveLiveMode { get; set; }
    public AuthenticationTokenConfigData? Authentication { get; set; }
}

internal record AuthenticationTokenConfigData
{
    public string? AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiry { get; set; }
    public string? RefreshToken { get; set; }

    public bool HasValidAccessToken() => !string.IsNullOrWhiteSpace(AccessToken) && AccessTokenExpiry > DateTimeOffset.UtcNow;
    public bool HasValidRefreshToken() => !string.IsNullOrWhiteSpace(RefreshToken);
}
