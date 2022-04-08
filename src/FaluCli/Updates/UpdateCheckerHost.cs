using Microsoft.Extensions.Options;

namespace Falu.Updates;

internal sealed class UpdateCheckerHost : BackgroundService
{
    private readonly UpdateCheckerOptions options;

    public UpdateCheckerHost(IOptions<UpdateCheckerOptions> optionsAccessor)
    {
        options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await UpdateChecker.CheckForUpdateAsync(options, stoppingToken);
        }
        catch { } // nothing to do here
    }
}
