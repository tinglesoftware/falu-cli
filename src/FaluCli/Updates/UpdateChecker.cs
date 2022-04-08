using Microsoft.Extensions.Options;
using Octokit;

namespace Falu.Updates;

internal sealed class UpdateChecker
{
    private readonly UpdateCheckerOptions options;
    private readonly IGitHubClient client;

    public UpdateChecker(IOptions<UpdateCheckerOptions> optionsAccessor)
    {
        options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        client = new GitHubClient(new ProductHeaderValue(options.ProductName));
    }

    public async Task<Version> GetLatestVersionAsync()
    {
        var release = await client.Repository.Release.GetLatest(options.RepositoryOwner, options.RepositoryName);
        var version = SemanticVersioning.Version.Parse(release.TagName ?? release.Name);
        return new Version(version.Major, version.Minor, version.Patch);
    }
}
