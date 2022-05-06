using System.Text.Json.Serialization;

namespace Falu.Client.Events;

internal class WebhookDeliveryAttempt
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("attempted")]
    public DateTimeOffset Attempted { get; set; }

    [JsonPropertyName("request_body")]
    public string? RequestBody { get; set; }

    [JsonPropertyName("http_status")]
    public int HttpStatus { get; set; }

    [JsonPropertyName("response_body")]
    public string? ResponseBody { get; set; }

    [JsonPropertyName("response_time")]
    public long ResponseTime { get; set; }

    [JsonPropertyName("successful")]
    public bool Successful { get; set; }
}
