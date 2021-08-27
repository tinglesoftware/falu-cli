using Falu;
using Falu.Core;
using FaluCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.CommandLine.Invocation;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IServiceCollectionExtensions
    {
        // get the version from the assembly
        private readonly static string ProductVersion = typeof(Program).Assembly.GetName().Version!.ToString(3);

        public static IServiceCollection AddFaluClientForCli(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFalu(configuration: configuration, configureBuilder: ConfigureHttpClient);

            services.AddSingleton<IConfigureOptions<FaluClientOptions>, ConfigureOptionsFaluClientOptions>();
            services.AddSingleton<IConfigureOptions<RequestOptions>, ConfigureOptionsRequestOptions>();

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

        internal class ConfigureOptionsFaluClientOptions : IConfigureOptions<FaluClientOptions>
        {
            private readonly InvocationContext context;

            public ConfigureOptionsFaluClientOptions(InvocationContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public void Configure(FaluClientOptions options)
            {
                var apiKey = context.ParseResult.ValueForOption<string>("--api-key");

                options.ApiKey = apiKey;
            }
        }

        internal class ConfigureOptionsRequestOptions : IConfigureOptions<RequestOptions>
        {
            private readonly InvocationContext context;

            public ConfigureOptionsRequestOptions(InvocationContext context)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public void Configure(RequestOptions options)
            {
                var workspaceId = context.ParseResult.ValueForOption<string>("--workspace-id");
                var live = context.ParseResult.ValueForOption<bool?>("--live");

                options.Workspace = workspaceId;
                // TODO: restore this
                //options.Live = live;
            }
        }
    }
}
