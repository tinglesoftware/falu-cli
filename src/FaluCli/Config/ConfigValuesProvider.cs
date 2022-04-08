namespace Falu.Config;

internal class ConfigValuesProvider : IConfigValuesProvider
{
    // Path example C:\Users\USERNAME\.config\falu\config.toml
    private static readonly string UserProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static readonly string FilePath = Path.Combine(UserProfileFolder, ".config", "falu", "config.toml");

    private ConfigValues? values;

    public ConfigValues GetConfigValues()
    {
        if (values is null)
        {
            var toml = File.ReadAllText(FilePath);
            values = Tomlyn.Toml.ToModel<ConfigValues>(toml);
        }
        return values;
    }

    public async ValueTask<ConfigValues> GetConfigValuesAsync(CancellationToken cancellationToken = default)
    {
        if (values is null)
        {
            var toml = await File.ReadAllTextAsync(FilePath, cancellationToken);
            values = Tomlyn.Toml.ToModel<ConfigValues>(toml);
        }
        return values;
    }

    public async Task SetConfigValuesAsync(ConfigValues values, CancellationToken cancellationToken = default)
    {
        Interlocked.Exchange(ref this.values, values);
        var toml = Tomlyn.Toml.FromModel(values);
        await File.WriteAllTextAsync(FilePath, toml, cancellationToken);
    }
}
