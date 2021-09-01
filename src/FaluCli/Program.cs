using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace FaluCli
{
    class Program
    {
        async static Task<int> Main(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                new Command("evaluations", "Manage evaluations.")
                {
                },

                new Command("messages", "Manage messages.")
                {
                },

                new Command("templates", "Manage message templates.")
                {
                    new Commands.Templates.PullTemplatesCommand(),
                },

                new Command("payments", "Manage payments.")
                {
                },

                new Command("events", "Work with events on Falu.")
                {
                    new Commands.Events.RetryCommand(),
                },

                new Command("webhooks", "Manage webhooks.")
                {
                },
            };

            rootCommand.Description = "Official CLI tool for Falu.";
            rootCommand.AddCommonGlobalOptions();

            var builder = new CommandLineBuilder(rootCommand)
                .UseHost(_ => Host.CreateDefaultBuilder(args), host =>
                {
                    host.ConfigureAppConfiguration((context, builder) =>
                    {
                        var iv = context.GetInvocationContext();
                        var verbose = iv.IsVerboseEnabled();

                        builder.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            ["Logging:LogLevel:Default"] = "Information",
                            ["Logging:LogLevel:Microsoft"] = "Warning",
                            ["Logging:LogLevel:System.Net.Http.HttpClient"] = "None", // removes all we do not need
                            ["Logging:LogLevel:System.Net.Http.HttpClient.FaluCliClient.LogicalHandler"] = verbose ? "Trace" : "Warning", // add the one we need
                        });
                    });

                    host.ConfigureServices((context, services) =>
                    {
                        var configuration = context.Configuration;
                        services.AddFaluClientForCli(configuration.GetSection("FaluClient"));
                    });

                    host.UseCommandHandler<Commands.Events.RetryCommand, Commands.Events.RetryCommandHandler>();
                    host.UseCommandHandler<Commands.Templates.PullTemplatesCommand, Commands.Templates.PullTemplatesCommandHandler>();
                })
                .UseFaluDefaults();

            // Parse the incoming args and invoke the handler
            var parser = builder.Build();
            return await parser.InvokeAsync(args);
        }
    }
}
