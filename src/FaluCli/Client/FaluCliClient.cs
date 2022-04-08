using Falu.Client.Events;
using Falu.Client.Money;
using Microsoft.Extensions.Options;

namespace Falu.Client;

internal class FaluCliClient : FaluClient
{
    public FaluCliClient(HttpClient backChannel, IOptionsSnapshot<FaluClientOptions> optionsAccessor)
        : base(backChannel, optionsAccessor)
    {
        Events = new ExtendedEventsServiceClient(BackChannel, Options);
        MoneyMpesa = new MoneyMpesaServiceClient(BackChannel, Options);
    }

    public new ExtendedEventsServiceClient Events { get; protected set; }

    public MoneyMpesaServiceClient MoneyMpesa { get; protected set; }
}
