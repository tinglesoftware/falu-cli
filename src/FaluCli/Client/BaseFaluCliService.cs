using Falu;
using Falu.Core;
using Falu.Infrastructure;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FaluCli.Client
{
    // TODO: find another way to work with FaluCliClientOptions
    internal class BaseFaluCliService : BaseService
    {
        public BaseFaluCliService(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

        protected override Task<HttpResponseMessage> RequestCoreAsync(HttpRequestMessage request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (Options is FaluCliClientOptions fcco)
            {
                options ??= new RequestOptions();
                options.Workspace = fcco.WorkspaceId;
                options.Live = fcco.Live;
            }

            return base.RequestCoreAsync(request, options, cancellationToken);
        }
    }
}
