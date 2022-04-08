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
                });

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
        services.AddHttpClient(Constants.OpenIdHttpClientName);

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
        private readonly InvocationContext context;

        public FaluClientConfigureOptions(InvocationContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Configure(FaluClientOptions options)
        {
            // TODO: pull from stored configuration
            options.ApiKey = context.ParseResult.ValueForOption<string>("--apikey");
        }
    }
}
