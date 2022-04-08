namespace Falu.Config;

internal interface IConfigValuesProvider
{
    ConfigValues GetConfigValues();
    ValueTask<ConfigValues> GetConfigValuesAsync(CancellationToken cancellationToken = default);
    Task SetConfigValuesAsync(ConfigValues values, CancellationToken cancellationToken = default);
}
