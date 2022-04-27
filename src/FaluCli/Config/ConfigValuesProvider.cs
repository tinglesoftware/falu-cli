using IdentityModel.Client;

namespace Falu.Config;

internal class ConfigValuesProvider : IConfigValuesProvider
{
    // Path example C:\Users\USERNAME\.config\falu\config.toml
    private static readonly string UserProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static readonly string FilePath = Path.Combine(UserProfileFolder, ".config", "falu", "config.toml");

    private ConfigValues? values;

    public async ValueTask<ConfigValues> GetConfigValuesAsync(CancellationToken cancellationToken = default)
    {
        if (values is null)
        {
            if (File.Exists(FilePath))
            {
                var toml = await File.ReadAllTextAsync(FilePath, cancellationToken);
                values = Tomlyn.Toml.ToModel<ConfigValues>(toml);
            }
            else
            {
                values = new ConfigValues();
            }
        }
        return values;
    }

    public async Task SaveConfigValuesAsync(CancellationToken cancellationToken = default)
    {
        // ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

        values ??= await GetConfigValuesAsync(cancellationToken);
        var toml = Tomlyn.Toml.FromModel(values);
        await File.WriteAllTextAsync(FilePath, toml, cancellationToken);
    }

    public async Task SaveConfigValuesAsync(TokenResponse response, CancellationToken cancellationToken = default)
    {
        values ??= await GetConfigValuesAsync(cancellationToken);
        values.Update(response);

        await SaveConfigValuesAsync(cancellationToken);
    }

    public async Task ClearAuthenticationAsync(CancellationToken cancellationToken = default)
    {
        values ??= await GetConfigValuesAsync(cancellationToken);
        if (values.Authentication is not null)
        {
            values.Authentication = null;
            await SaveConfigValuesAsync(cancellationToken);
        }
    }

    public void ClearAll()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }
}
