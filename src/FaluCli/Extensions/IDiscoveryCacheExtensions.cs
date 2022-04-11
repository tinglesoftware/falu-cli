using Falu.Commands.Login;

namespace IdentityModel.Client;

internal static class IDiscoveryCacheExtensions
{
    public static async Task<DiscoveryDocumentResponse> GetSafelyAsync(this IDiscoveryCache cache, CancellationToken cancellationToken = default)
    {
        var response = await cache.GetAsync();
        if (response.IsError) throw new LoginException(response);

        return response;
    }
}
