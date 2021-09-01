using Falu;
using Falu.Core;
using Falu.Infrastructure;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FaluCli.Client.Events
{
    internal class EventsServiceForCli : Falu.Events.EventsService
    {
        public EventsServiceForCli(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

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
