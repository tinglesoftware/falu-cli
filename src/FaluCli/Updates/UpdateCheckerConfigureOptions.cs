using Microsoft.Extensions.Options;

namespace Falu.Updates;

internal class UpdateCheckerConfigureOptions : IPostConfigureOptions<UpdateCheckerOptions>, IValidateOptions<UpdateCheckerOptions>
{
    public void PostConfigure(string name, UpdateCheckerOptions options)
    {
        options.ProductName ??= "falu-cli";
        options.RepositoryOwner ??= "tinglesoftware";
        options.RepositoryName ??= "falu-cli";
    }

    public ValidateOptionsResult Validate(string name, UpdateCheckerOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ProductName))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.ProductName)}' must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.RepositoryOwner))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.RepositoryOwner)}' must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.RepositoryName))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.RepositoryName)}' must be provided.");
        }

        return ValidateOptionsResult.Success;
    }
}
