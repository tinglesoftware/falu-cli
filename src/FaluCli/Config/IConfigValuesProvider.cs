using IdentityModel.Client;

namespace Falu.Config;

internal interface IConfigValuesProvider
{
    ValueTask<ConfigValues> GetConfigValuesAsync(CancellationToken cancellationToken = default);
    Task SaveConfigValuesAsync(CancellationToken cancellationToken = default);
    Task SaveConfigValuesAsync(TokenResponse response, CancellationToken cancellationToken = default);

    Task ClearAuthenticationAsync(CancellationToken cancellationToken = default);
    void ClearAll();
}
