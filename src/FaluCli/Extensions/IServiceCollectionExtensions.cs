using Falu;
using Falu.Client;
using Falu.Config;
using Falu.Updates;
using IdentityModel.Client;
using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddFaluClientForCli(this IServiceCollection services)
    {
        // A dummy ApiKey is used so that the options validator can pass
        services.AddFalu<FaluCliClient, FaluClientOptions>(o => o.ApiKey = "dummy")
                .AddHttpMessageHandler<FaluCliClientHandler>()
                .AddHttpMessageHandler<HttpAuthenticationHandler>()
                .ConfigureHttpClient(client =>
                {
                    // change the User-Agent header
                    client.DefaultRequestHeaders.UserAgent.Clear();
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("falucli", VersioningHelper.ProductVersion));
                });

        services.AddScoped<FaluCliClientHandler>();
        services.AddScoped<HttpAuthenticationHandler>();

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
}
