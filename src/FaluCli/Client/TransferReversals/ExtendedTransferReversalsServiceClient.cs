using Falu.Client.Money;
using Falu.Core;
using Falu.TransferReversals;

namespace Falu.Client.TransferReversals;

internal class ExtendedTransferReversalsServiceClient : TransferReversalsServiceClient, ISupportsUploadingMpesaStatement
{
    public ExtendedTransferReversalsServiceClient(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

    #region ISupportsUploadingMpesaStatement members

    string ISupportsUploadingMpesaStatement.ObjectKind => "transfer_reversals";

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
