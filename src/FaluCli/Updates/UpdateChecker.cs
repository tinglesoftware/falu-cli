using Octokit;

namespace Falu.Updates;

internal static class UpdateChecker
{
    private static readonly SemaphoreSlim locker = new(1);
    private static SemanticVersioning.Version? latestVersion, currentVersion;

    public static async Task CheckForUpdateAsync(UpdateCheckerOptions options, CancellationToken cancellationToken = default)
    {
        if (latestVersion is not null) return;

        try
        {
            var client = new GitHubClient(new ProductHeaderValue(options.ProductName));
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
