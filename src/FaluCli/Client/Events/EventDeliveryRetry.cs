using System.Text.Json.Serialization;

namespace Falu.Client.Events;

public class EventDeliveryRetry
{
    [JsonPropertyName("webhook_endpoint_id")]
    public string? WebhookEndpointId { get; set; }
}
