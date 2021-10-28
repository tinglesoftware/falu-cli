using Falu;
using FaluCli.Client.Events;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FaluCli.Client
{
    internal class FaluCliClient : FaluClient
    {
        public FaluCliClient(HttpClient backChannel, IOptionsSnapshot<FaluClientOptions> optionsAccessor)
            : base(backChannel, optionsAccessor)
        {
            Events = new ExtendedEventsServiceClient(BackChannel, Options);
        }

        public new ExtendedEventsServiceClient Events { get; protected set; }
    }
}
