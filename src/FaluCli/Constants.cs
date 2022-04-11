using System.Text.RegularExpressions;

namespace Falu;

internal class Constants
{
    public const string OpenIdCategoryOrClientName = "Oidc";

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

    public static readonly ByteSizeLib.ByteSize MaxMpesaStatementFileSize = ByteSizeLib.ByteSize.FromKibiBytes(128);
    public static readonly string MaxMpesaStatementFileSizeString = MaxMpesaStatementFileSize.ToBinaryString();

    public static readonly Regex ObjectIdFormat = new(@"^[a-z]{2,7}_[a-zA-Z0-9]{20,30}$");
}
