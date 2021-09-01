﻿using FaluCli.Client;
using FaluCli.Client.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace FaluCli.Commands.Events
{
    internal class RetryCommandHandler : ICommandHandler
    {
        public FaluCliClient client;
        private readonly ILogger logger;

        public RetryCommandHandler(FaluCliClient client, ILogger<RetryCommandHandler> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var cancellationToken = context.GetCancellationToken();
            var eventId = context.ParseResult.ValueForArgument<string>("event");
            var webhookEndpointId = context.ParseResult.ValueForOption<string>("--webhook-endpoint");

            var model = new EventDeliveryRetry { WebhookEndpointId = webhookEndpointId, };
            var response = await client.EventsCli.RetryAsync(eventId!, model, cancellationToken: cancellationToken);
            response.EnsureSuccess();

            var attempt = response.Resource!;

            var time = TimeSpan.FromMilliseconds(attempt.ResponseTime);
            var data = new Dictionary<string, object?>
            {
                ["Attempted"] = attempt.Attempted,
                ["Url"] = attempt.Url,
                ["Response Time"] = $"{time.TotalSeconds:n3} seconds",
            };

            if (!attempt.Successful)
            {
                var statusCode = Enum.Parse<HttpStatusCode>(attempt.HttpStatus.ToString());
                data["Http Status"] = $"{statusCode} ({attempt.HttpStatus})";
                if (context.IsVerboseEnabled())
                {
                    data["Request Body"] = PrettyJson(attempt.RequestBody!);
                    data["Response Body"] = attempt.ResponseBody;
                }
            }

            var str = $"{(attempt.Successful ? "Retry succeeded." : "Retry failed!")}\r\n";
            str += data.RemoveDefaultAndEmpty().MakePaddedString();
            logger.LogInformation(str);

            return 0;
        }

        private static string PrettyJson(string json)
        {
            var so = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            var je = JsonSerializer.Deserialize<JsonElement>(json);
            return JsonSerializer.Serialize(je, so);
        }
    }
}