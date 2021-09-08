using Falu;
using FaluCli.Client.Events;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FaluCli.Client
{
    internal class FaluCliClient : FaluClient
    {
        public FaluCliClient(HttpClient backChannel, IOptions<FaluClientOptions> optionsAccessor)
            : base(backChannel, optionsAccessor)
        {
            Events = new EventsServiceForCli(BackChannel, Options);
        }

        public new EventsServiceForCli Events { get; protected set; }
    }
}
