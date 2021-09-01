using Falu.MessageTemplates;
using FaluCli.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FaluCli.Commands.Templates
{
    internal class TemplatesCommandHandler : ICommandHandler
    {
        public FaluCliClient client;
        private readonly ILogger logger;

        public TemplatesCommandHandler(FaluCliClient client, ILogger<TemplatesCommandHandler> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<int> InvokeAsync(InvocationContext context)
        {
            var command = context.ParseResult.CommandResult.Command;
            if (command is PullTemplatesCommand) return HandlePullAsync(context);
            else if (command is PushTemplatesCommand) return HandlePushAsync(context);
            throw new InvalidOperationException($"Command of type '{command.GetType().FullName}' is not supported here.");
        }

        private async Task<int> HandlePullAsync(InvocationContext context)
        {
            var cancellationToken = context.GetCancellationToken();
            var outputPath = context.ParseResult.ValueForArgument<string>("output-directory")!;
            var overwrite = context.ParseResult.ValueForOption<bool>("--overwrite");

            // download the templates
            logger.LogInformation("Fetching templates ...");
            var templates = await DownloadTemplatesAsync(cancellationToken);
            logger.LogInformation("Received {TemplatesCount} templates.", templates.Count);

            // work on each template
            var saved = 0;
            foreach (var template in templates)
            {
                if (string.IsNullOrWhiteSpace(template.Alias))
                {
                    logger.LogWarning("Template '{TemplateId}' without an alias shall be skipped.", template.Id);
                    continue;
                }

                await SaveTemplateAsync(template, outputPath, overwrite, cancellationToken);
                saved++;
            }

            logger.LogInformation("Finished saving {Save} of {Total} templates to {OutputDirectory}", saved, templates.Count, outputPath);

            return 0;
        }

        /// <inheritdoc/>
        public async Task<int> HandlePushAsync(InvocationContext context)
        {
            var cancellationToken = context.GetCancellationToken();
            var all = context.ParseResult.ValueForOption<bool>("--all");

            return 0;
        }
        private async Task<IReadOnlyList<MessageTemplate>> DownloadTemplatesAsync(CancellationToken cancellationToken)
        {
            var result = new List<MessageTemplate>();
            var options = new MessageTemplatesListOptions { Count = 100, };
            var templates = client.MessageTemplates.ListRecursivelyAsync(options, cancellationToken: cancellationToken);
            await foreach (var template in templates)
            {
                result.Add(template);
            }

            return result;
        }

        private async Task SaveTemplateAsync(MessageTemplate template, string outputPath, bool overwrite, CancellationToken cancellationToken)
        {
            // create directory if it does not exist
            var dirPath = Path.Combine(outputPath, template.Alias!);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            // write the template body
            var contentPath = Path.Combine(dirPath, "content.txt");
            await WriteToFileAsync(contentPath, overwrite, template.Body!, cancellationToken);

            // write the template metadata
            var metadata = new TemplateMetadata(template);
            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var metadataJson = JsonSerializer.Serialize(metadata, serializerOptions);
            var metdataPath = Path.Combine(dirPath, "meta.json");
            await WriteToFileAsync(metdataPath, overwrite, metadataJson, cancellationToken);
        }

        private async Task WriteToFileAsync(string path, bool overwrite, string contents, CancellationToken cancellationToken)
        {
            var exists = File.Exists(path);
            if (exists && !overwrite)
            {
                logger.LogWarning("Skipping overwrite for {Path}", path);
                return;
            }

            await File.WriteAllTextAsync(path, contents, cancellationToken);
        }
    }
}
