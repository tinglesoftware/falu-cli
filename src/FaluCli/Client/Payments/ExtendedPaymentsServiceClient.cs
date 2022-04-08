using Falu.Core;
using Falu.Payments;
using System.IO;

namespace Falu.Client.Payments;

internal class ExtendedPaymentsServiceClient : PaymentsServiceClient
{
    public ExtendedPaymentsServiceClient(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

    public virtual Task<ResourceResponse<List<ExtractedMpesaStatementRecord>>> UploadMpesaAsync(string fileName,
                                                                                                Stream fileContent,
                                                                                                RequestOptions? options = null,
                                                                                                CancellationToken cancellationToken = default)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(fileContent), "file", fileName }
        };
        var uri = "/v1/money/mpesa/statements/upload/payments";
        return RequestAsync<List<ExtractedMpesaStatementRecord>>(uri, HttpMethod.Post, content, options, cancellationToken);
    }
}
