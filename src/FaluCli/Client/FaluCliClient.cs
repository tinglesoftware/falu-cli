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
            EventsCli = new EventsServiceForCli(BackChannel, Options);
        }

        internal EventsServiceForCli EventsCli { get; }
    }
}
