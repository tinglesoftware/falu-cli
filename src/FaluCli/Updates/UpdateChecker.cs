using Octokit;

namespace Falu.Updates;

internal class UpdateChecker : BackgroundService
{
    public const string RepositoryOwner = "tinglesoftware";
    public const string RepositoryName = "falu-cli";

    private static readonly SemaphoreSlim locker = new(1);
    private static readonly SemanticVersioning.Version? currentVersion = SemanticVersioning.Version.Parse(VersioningHelper.ProductVersion);
    private static SemanticVersioning.Version? latestVersion;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (latestVersion is not null) return;

            try
            {
                var client = new GitHubClient(new ProductHeaderValue(RepositoryName));
                await locker.WaitAsync(stoppingToken);
                var release = await client.Repository.Release.GetLatest(RepositoryOwner, RepositoryName);
                Interlocked.Exchange(ref latestVersion, SemanticVersioning.Version.Parse(release.TagName));
            }
            finally
            {
                locker.Release();
            }
        }
        catch { } // nothing to do here
    }

    public static SemanticVersioning.Version? LatestVersion => latestVersion;
    public static SemanticVersioning.Version? CurrentVersion => currentVersion;
    public static bool HasUpdate => LatestVersion > CurrentVersion;
}
