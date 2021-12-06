using Falu;
using Falu.Core;
using Falu.Events;

namespace FaluCli.Client.Events
{
    internal class ExtendedEventsServiceClient : EventsServiceClient
    {
        public ExtendedEventsServiceClient(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

        public virtual Task<ResourceResponse<WebhookDeliveryAttempt>> RetryAsync(string id,
                                                                                 EventDeliveryRetry model,
                                                                                 RequestOptions? options = null,
                                                                                 CancellationToken cancellationToken = default)
        {
            var uri = MakeResourcePath(id) + "/retry";
            return RequestAsync<WebhookDeliveryAttempt>(uri, HttpMethod.Post, model, options, cancellationToken);
        }
    }
}
