using Falu;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace FaluCli.Client
{
    internal class FaluCliClient : FaluClient<FaluCliClientOptions>
    {
        public FaluCliClient(HttpClient backChannel, IOptions<FaluCliClientOptions> optionsAccessor)
            : base(backChannel, optionsAccessor)
        {
        }
    }
}
