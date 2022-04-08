using Falu.Core;
using System.IO;

namespace Falu.Client.Money;

internal class MoneyMpesaServiceClient : BaseServiceClient
{
    private const string BasePath = "/v1/money/mpesa/statements";

    public MoneyMpesaServiceClient(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

    public virtual Task<ResourceResponse<List<ExtractedMpesaStatementRecord>>> UploadPaymentsAsync(string fileName,
                                                                                                   Stream fileContent,
                                                                                                   RequestOptions? options = null,
                                                                                                   CancellationToken cancellationToken = default)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(fileContent), "file", fileName }
        };
        var uri = $"{BasePath}/upload/payments";
        return RequestAsync<List<ExtractedMpesaStatementRecord>>(uri, HttpMethod.Post, content, options, cancellationToken);
    }

    public virtual Task<ResourceResponse<List<ExtractedMpesaStatementRecord>>> UploadTransfersAsync(string fileName,
                                                                                                    Stream fileContent,
                                                                                                    RequestOptions? options = null,
                                                                                                    CancellationToken cancellationToken = default)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(fileContent), "file", fileName }
        };
        var uri = $"{BasePath}/upload/transfers";
        return RequestAsync<List<ExtractedMpesaStatementRecord>>(uri, HttpMethod.Post, content, options, cancellationToken);
    }
}
