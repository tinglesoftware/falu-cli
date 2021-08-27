using Falu;
using Falu.Core;
using FaluCli;
using FaluCli.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;

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
                // populate the User-Agent header
                var userAgent = $"falucli/{ProductVersion}";
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
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
                var apiKey = context.ParseResult.ValueForOption<string>("--api-key");

                options.ApiKey = apiKey;
                var workspaceId = context.ParseResult.ValueForOption<string>("--workspace-id");
                var live = context.ParseResult.ValueForOption<bool?>("--live");

                options.WorkspaceId = workspaceId;
                options.Live = live;
            }
        }
    }
}
