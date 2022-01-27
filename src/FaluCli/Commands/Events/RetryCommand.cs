namespace Falu.Commands.Events;

internal class RetryCommand : Command
{
    public RetryCommand() : base("retry", "Retry delivery of an event to a webhook endpoint.")
    {
        // TODO: validate eventId using regex -> "^evt_[0-9a-f]{24}$"
        this.AddArgument<string>(name: "event",
                                 description: "Unique identifier of the event. Example: evt_610010be9228355f14ce6e08");

        // TODO: validate webhookEndpointId using regex -> "^we_[0-9a-f]{24}$"
        this.AddOption<string>(aliases: new[] { "--webhook-endpoint" },
                               description: "Unique identifier of the webhook endpoint. Example: we_610010be9228355f14ce6e08",
                               configure: o => o.IsRequired = true);
    }
}
