using Microsoft.Extensions.Options;
using Octokit;

namespace Falu.Updates;

internal class UpdateChecker : BackgroundService
{
    private static readonly SemaphoreSlim locker = new(1);
    private static SemanticVersioning.Version? latestVersion, currentVersion;

    private readonly UpdateCheckerOptions options;

    public UpdateChecker(IOptions<UpdateCheckerOptions> optionsAccessor)
    {
        options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (latestVersion is not null) return;

            try
            {
                var client = new GitHubClient(new ProductHeaderValue(options.RepositoryName));
                await locker.WaitAsync(stoppingToken);
                var release = await client.Repository.Release.GetLatest(options.RepositoryOwner, options.RepositoryName);
                Interlocked.Exchange(ref latestVersion, SemanticVersioning.Version.Parse(release.TagName));
                Interlocked.Exchange(ref currentVersion, SemanticVersioning.Version.Parse(options.CurrentVersion));
            }
            finally
            {
                locker.Release();
            }
        }
        catch { } // nothing to do here
    }

    public static async Task CheckForUpdateAsync(UpdateCheckerOptions options, CancellationToken cancellationToken = default)
    {
        if (latestVersion is not null) return;

        try
        {
            var client = new GitHubClient(new ProductHeaderValue(options.RepositoryName));
            await locker.WaitAsync(cancellationToken);
            var release = await client.Repository.Release.GetLatest(options.RepositoryOwner, options.RepositoryName);
            Interlocked.Exchange(ref latestVersion, SemanticVersioning.Version.Parse(release.TagName));
            Interlocked.Exchange(ref currentVersion, SemanticVersioning.Version.Parse(options.CurrentVersion));
        }
        finally
        {
            locker.Release();
        }
    }

    public static SemanticVersioning.Version? LatestVersion => latestVersion;
    public static SemanticVersioning.Version? CurrentVersion => currentVersion;
    public static bool HasUpdate => LatestVersion > CurrentVersion;
}
