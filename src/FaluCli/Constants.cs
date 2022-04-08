namespace Falu;

internal class Constants
{
    public const string OpenIdHttpClientName = "OpenIdClient";

    public const string RepositoryOwner = "tinglesoftware";
    public const string RepositoryName = "falu-cli";

    public const string Authority = "https://login.falu.io";
    public const string ScopeApi = "api";
    public const string ClientId = "cli";
    public static readonly ICollection<string> ScopesList = new HashSet<string>
    {
        IdentityModel.OidcConstants.StandardScopes.OpenId,
        IdentityModel.OidcConstants.StandardScopes.OfflineAccess,
        ScopeApi,
    };
    public static readonly string Scopes = string.Join(" ", ScopesList);
}
