using Falu.Config;
using Microsoft.Extensions.Options;

namespace Falu.Client;

internal class FaluClientConfigureOptions : IConfigureOptions<FaluClientOptions>
{
    private readonly IConfigValuesProvider configValuesProvider;

    public FaluClientConfigureOptions(IConfigValuesProvider configValuesProvider)
    {
        this.configValuesProvider = configValuesProvider ?? throw new ArgumentNullException(nameof(configValuesProvider));
    }

    public void Configure(FaluClientOptions options)
    {
        var values = configValuesProvider.GetConfigValuesAsync().GetAwaiter().GetResult();
        options.Retries = values.Retries;
    }
}
