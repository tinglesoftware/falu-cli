using Falu;
using Falu.Core;
using Falu.Infrastructure;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FaluCli.Client.Events
{
    internal class EventsCliService : BaseFaluCliService
    {
        public EventsCliService(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

        public virtual async Task<ResourceResponse<WebhookDeliveryAttempt>> RetryAsync(string id,
                                                                                       EventDeliveryRetry model,
                                                                                       RequestOptions? options = null,
                                                                                       CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));

            var uri = new Uri(BaseAddress, $"/v1/events/{id}/retry");
            return await PostAsync<WebhookDeliveryAttempt>(uri, model, options, cancellationToken);
        }
    }
}
