using Microsoft.Extensions.Options;

namespace Falu.Updates;

internal sealed class UpdateCheckerHost : BackgroundService
{
    private readonly UpdateCheckerOptions options;
    private readonly ILogger logger;

    public UpdateCheckerHost(IHostApplicationLifetime lifetime, IOptions<UpdateCheckerOptions> optionsAccessor, ILoggerFactory loggerFactory)
    {
        options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        logger = loggerFactory.CreateLogger("");
        lifetime.ApplicationStopping.Register(ApplicationStopping);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await UpdateChecker.CheckForUpdateAsync(options, stoppingToken);
        }
        catch { } // nothing to do here
    }

    private void ApplicationStopping()
    {
        var latest = UpdateChecker.LatestVersion;
        if (latest > UpdateChecker.CurrentVersion)
        {
            // https://github.com/tinglesoftware/falu-cli/releases/tag/0.3.0
            var url = "https://" + "github.com/" + options.RepositoryOwner + "/" + options.RepositoryName + "/tag/" + latest;
            logger.LogInformation("New version {LatestVersion} is available.\r\nDownload at {Url}", latest, url);
        }
    }
}
