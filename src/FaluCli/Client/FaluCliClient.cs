using Falu.Client.Events;
using Microsoft.Extensions.Options;

namespace Falu.Client;

internal class FaluCliClient : FaluClient
{
    public FaluCliClient(HttpClient backChannel, IOptionsSnapshot<FaluClientOptions> optionsAccessor)
        : base(backChannel, optionsAccessor)
    {
        Events = new ExtendedEventsServiceClient(BackChannel, Options);
    }

    public new ExtendedEventsServiceClient Events { get; protected set; }
}
