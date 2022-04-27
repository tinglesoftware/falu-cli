using Falu.Config;

namespace Falu.Commands.Config;

internal class ConfigCommandHandler : ICommandHandler
{
    private readonly IConfigValuesProvider configValuesProvider;
    private readonly ILogger logger;

    public ConfigCommandHandler(IConfigValuesProvider configValuesProvider, ILogger<ConfigCommandHandler> logger)
    {
        this.configValuesProvider = configValuesProvider ?? throw new ArgumentNullException(nameof(configValuesProvider));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();

        switch (context.ParseResult.CommandResult.Command)
        {
            case ConfigShowCommand:
                {
                    var values = await configValuesProvider.GetConfigValuesAsync(cancellationToken);
                    var data = new Dictionary<string, object?>
                    {
                        ["ActiveWorkspaceId"] = values.ActiveWorkspaceId,
                        ["ActiveLiveMode"] = values.ActiveLiveMode,
                    };

                    var str = data.RemoveDefaultAndEmpty().MakePaddedString("=");
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        logger.LogInformation("Configuration values are empty or only contain sensitive information.");
                    }
                    else
                    {
                        logger.LogInformation("Configuration values:\r\n{Values}", str);
                    }

                    break;
                }
            case ConfigClearAuthenticationCommand:
                {
                    logger.LogInformation("Removing authentication configuration ...");
                    await configValuesProvider.ClearAuthenticationAsync(cancellationToken);
                    logger.LogInformation("Successfully removed all authentication configuration values.");
                    break;
                }
            case ConfigClearAllCommand:
                {
                    logger.LogInformation("Clearing all configuration values ...");
                    configValuesProvider.ClearAll();
                    logger.LogInformation("Successfully removed all configuration values and the configuration file.");
                    break;
                }
        }

        return 0;
    }
}
