using Falu.Client.Money;
using Falu.Core;
using Falu.Transfers;

namespace Falu.Client.Transfers;

internal class ExtendedTransfersServiceClient : TransfersServiceClient, ISupportsUploadingMpesaStatement
{
    public ExtendedTransfersServiceClient(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

    #region ISupportsUploadingMpesaStatement members

    string ISupportsUploadingMpesaStatement.ObjectKind => "transfers";

    Task<ResourceResponse<TResource>> ISupportsUploadingMpesaStatement.RequestAsync<TResource>(string uri,
                                                                                               HttpMethod method,
                                                                                               HttpContent? content,
                                                                                               RequestOptions? options,
                                                                                               CancellationToken cancellationToken)
    {
        return base.RequestAsync<TResource>(uri, method, content, options, cancellationToken);
    }

    #endregion
}
