using Octokit;

namespace Falu.Updates;

internal class UpdateChecker : BackgroundService
{
    private static readonly SemaphoreSlim locker = new(1);
    private static readonly SemanticVersioning.Version? currentVersion = SemanticVersioning.Version.Parse(VersioningHelper.ProductVersion);
    private static SemanticVersioning.Version? latestVersion;
    private static string? latestVersionHtmlUrl;

    private readonly IHostEnvironment environment;

    public UpdateChecker(IHostEnvironment environment)
    {
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (environment.IsDevelopment() || latestVersion is not null) return;

            try
            {
                var client = new GitHubClient(new ProductHeaderValue(Constants.RepositoryName));
                await locker.WaitAsync(stoppingToken);
                var release = await client.Repository.Release.GetLatest(Constants.RepositoryOwner, Constants.RepositoryName);
                Interlocked.Exchange(ref latestVersion, SemanticVersioning.Version.Parse(release.TagName));
                Interlocked.Exchange(ref latestVersionHtmlUrl, release.HtmlUrl);
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
    public static string? LatestVersionHtmlUrl => latestVersionHtmlUrl;
}
