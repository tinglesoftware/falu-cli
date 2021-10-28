using Falu;
using FaluCli;
using FaluCli.Client;
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

        public static IServiceCollection AddFaluClientForCli(this IServiceCollection services)
        {
            services.AddFalu<FaluCliClient, FaluClientOptions>()
                    .AddHttpMessageHandler(provider => ActivatorUtilities.CreateInstance<FaluCliClientHandler>(provider))
                    .ConfigureHttpClient(client =>
                    {
                        // TODO: remove this once we migrate to using the library and not gitsubmodule since it will have correct value
                        client.DefaultRequestHeaders.UserAgent.Clear();

                        // populate the User-Agent header
                        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("falucli", ProductVersion));
                    });

            services.AddSingleton<IConfigureOptions<FaluClientOptions>, ConfigureFaluClientOptions>();

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
}
