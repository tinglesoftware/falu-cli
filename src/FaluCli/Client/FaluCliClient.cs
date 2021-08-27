using Falu;
using FaluCli.Client.Events;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FaluCli.Client
{
    internal class FaluCliClient : FaluClient<FaluCliClientOptions>
    {
        public FaluCliClient(HttpClient backChannel, IOptions<FaluCliClientOptions> optionsAccessor)
            : base(backChannel, optionsAccessor)
        {
            EventsCli = new EventsCliService(BackChannel, Options);
        }

        internal EventsCliService EventsCli { get; }
    }
}
