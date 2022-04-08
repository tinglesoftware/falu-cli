using IdentityModel.Client;

namespace Falu.Config;

internal interface IConfigValuesProvider
{
    ConfigValues GetConfigValues();
    ValueTask<ConfigValues> GetConfigValuesAsync(CancellationToken cancellationToken = default);
    Task SaveConfigValuesAsync(CancellationToken cancellationToken = default);
    Task SaveConfigValuesAsync(TokenResponse response, CancellationToken cancellationToken = default);
}
