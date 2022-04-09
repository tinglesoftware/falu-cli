using Falu;

namespace System.Net.Http;

internal static class IHttpClientFactoryExtensions
{
    public static HttpClient CreateOpenIdClient(this IHttpClientFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));
        return factory.CreateClient(Constants.OpenIdCategoryOrClientName);
    }
}
