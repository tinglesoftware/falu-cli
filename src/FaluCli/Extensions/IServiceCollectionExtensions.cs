using FaluCli;
using FaluCli.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;
using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IServiceCollectionExtensions
    {
        // get the version from the assembly
        private static readonly string ProductVersion = typeof(Program).Assembly.GetName().Version!.ToString(3);

        public static IServiceCollection AddFaluClientForCli(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFaluInner<FaluCliClient, FaluCliClientOptions>(configuration: configuration, configureBuilder: ConfigureHttpClient);

            services.AddSingleton<IConfigureOptions<FaluCliClientOptions>, ConfigureFaluCliClientOptions>();

            return services;
        }

        private static void ConfigureHttpClient(IHttpClientBuilder builder)
        {
            builder.ConfigureHttpClient(client =>
            {
                // TODO: remove this once we migrate to using the library and not gitsubmodule since it will have correct value
                client.DefaultRequestHeaders.UserAgent.Clear();

                // populate the User-Agent header
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("falucli", ProductVersion));
            });
        }

        internal class ConfigureFaluCliClientOptions : IConfigureOptions<FaluCliClientOptions>
        {
            private readonly InvocationContext context;

            public ConfigureFaluCliClientOptions(InvocationContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public void Configure(FaluCliClientOptions options)
            {
                var apiKey = context.ParseResult.ValueForOption<string>("--apikey");

                options.ApiKey = apiKey;
                var workspaceId = context.ParseResult.ValueForOption<string>("--workspace");
                var live = context.ParseResult.ValueForOption<bool?>("--live");

                options.WorkspaceId = workspaceId;
                options.Live = live;
            }
        }
    }
}
