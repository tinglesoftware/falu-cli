using FaluCli.Client;
using FaluCli.Client.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
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
            var webhookEndpointId = context.ParseResult.ValueForOption<string>("--webhook-endpoint-id");
            var eventId = context.ParseResult.ValueForOption<string>("--event-id");

            var model = new EventDeliveryRetry { WebhookEndpointId = webhookEndpointId, };
            var response = await client.EventsCli.RetryAsync(eventId!, model, cancellationToken: cancellationToken);
            response.EnsureSuccess();

            var attempt = response.Resource!;

            var time = TimeSpan.FromMilliseconds(attempt.ResponseTime);
            var data = new Dictionary<string, object?>
            {
                ["Attempted"] = attempt.Attempted,
                ["Url"] = attempt.Url!,
                ["Response Time"] = $"{time.TotalSeconds:n3} seconds",
            };

            if (!attempt.Successful)
            {
                data["Http Status"] = attempt.HttpStatus;
                data["Request Body"] = attempt.RequestBody!;
                data["Response Body"] = attempt.ResponseBody!;
            }

            var str = $"{(attempt.Successful ? "Retry succeeded." : "Retry failed!")}\r\n";
            str += data.RemoveDefaultAndEmpty().MakePaddedString();
            logger.LogInformation(str);

            return 0;
        }
    }
}
