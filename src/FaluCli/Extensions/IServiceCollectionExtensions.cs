using Falu;
using Falu.Client;
using Falu.Config;
using Falu.Updates;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddFaluClientForCli(this IServiceCollection services)
    {
        services.AddFalu<FaluCliClient, FaluClientOptions>()
                .AddHttpMessageHandler(provider => ActivatorUtilities.CreateInstance<FaluCliClientHandler>(provider))
                .ConfigureHttpClient(client =>
                {
                    // change the User-Agent header
                    client.DefaultRequestHeaders.UserAgent.Clear();
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("falucli", VersioningHelper.ProductVersion));
                })
                .AddHttpMessageHandler<HttpAuthenticationHandler>();

        services.AddScoped<HttpAuthenticationHandler>();
        services.ConfigureOptions<FaluClientConfigureOptions>();

        return services;
    }

    public static IServiceCollection AddUpdateChecker(this IServiceCollection services)
    {
        return services.AddHostedService<UpdateChecker>();
    }

    public static IServiceCollection AddConfigValuesProvider(this IServiceCollection services)
    {
        return services.AddScoped<IConfigValuesProvider, ConfigValuesProvider>();
    }

    public static IServiceCollection AddOpenIdServices(this IServiceCollection services)
    {
        services.AddHttpClient(Constants.OpenIdCategoryOrClientName);

        services.AddScoped<IDiscoveryCache>(p =>
        {
            var httpClientFactory = p.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateOpenIdClient();
            return new DiscoveryCache(Constants.Authority, () => client);
        });

        return services;
    }

    internal class FaluClientConfigureOptions : IConfigureOptions<FaluClientOptions>
    {
        public void Configure(FaluClientOptions options)
        {
            options.ApiKey = Constants.DefaultApiKey;
        }
    }
}
