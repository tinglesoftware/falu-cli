using Falu;
using Falu.Client;
using Falu.Updates;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

internal static class IServiceCollectionExtensions
{
    // get the version from the assembly
    private static readonly AssemblyName AssemblyName = typeof(Program).Assembly.GetName();
    private static readonly string ProductVersion = AssemblyName.Version!.ToString(3);

    public static IServiceCollection AddFaluClientForCli(this IServiceCollection services)
    {
        services.AddFalu<FaluCliClient, FaluClientOptions>()
                .AddHttpMessageHandler(provider => ActivatorUtilities.CreateInstance<FaluCliClientHandler>(provider))
                .ConfigureHttpClient(client =>
                {
                    // change the User-Agent header
                    client.DefaultRequestHeaders.UserAgent.Clear();
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("falucli", ProductVersion));
                });

        services.AddSingleton<IConfigureOptions<FaluClientOptions>, ConfigureFaluClientOptions>();

        return services;
    }

    public static IServiceCollection AddUpdateChecker(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UpdateCheckerOptions>(o => o.CurrentVersion = ProductVersion);
        services.Configure<UpdateCheckerOptions>(configuration);
        services.ConfigureOptions<UpdateCheckerConfigureOptions>();
        services.AddHostedService<UpdateCheckerHost>();

        return services;
    }

    internal class ConfigureFaluClientOptions : IConfigureOptions<FaluClientOptions>
    {
        private readonly InvocationContext context;

        public ConfigureFaluClientOptions(InvocationContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Configure(FaluClientOptions options)
        {
            options.ApiKey = context.ParseResult.ValueForOption<string>("--apikey");
        }
    }
}
