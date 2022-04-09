using Falu.Commands.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityModel.Client;

internal static class IDiscoveryCacheExtensions
{
    public static async Task<DiscoveryDocumentResponse> GetSafelyAsync(this IDiscoveryCache cache, CancellationToken cancellationToken = default)
    {
        var response = await cache.GetAsync();
        if (response.IsError) throw new LoginException(response.Error, response.Exception);

        return response;
    }
}
