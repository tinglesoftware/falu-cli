using Falu;

namespace Microsoft.Extensions.Logging;

internal static class ILoggerFactoryExtensions
{
    public static ILogger CreateOpenIdLogger(this ILoggerFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));
        return factory.CreateLogger(Constants.OpenIdCategoryOrClientName);
    }
}
