namespace FaluCli.Client.Events
{
    internal class WebhookDeliveryAttempt
    {
        public string? Url { get; set; }
        public DateTimeOffset Attempted { get; set; }
        public string? RequestBody { get; set; }
        public int HttpStatus { get; set; }
        public string? ResponseBody { get; set; }
        public long ResponseTime { get; set; }
        public bool Successful { get; set; }
    }
}
