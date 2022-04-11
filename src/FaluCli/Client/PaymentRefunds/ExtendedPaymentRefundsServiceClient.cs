using Falu.Client.Money;
using Falu.Core;
using Falu.PaymentRefunds;

namespace Falu.Client.PaymentRefunds;

internal class ExtendedPaymentRefundsServiceClient : PaymentRefundsServiceClient, ISupportsUploadingMpesaStatement
{
    public ExtendedPaymentRefundsServiceClient(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

    #region ISupportsUploadingMpesaStatement members

    string ISupportsUploadingMpesaStatement.ObjectKind => "payment_refunds";

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
