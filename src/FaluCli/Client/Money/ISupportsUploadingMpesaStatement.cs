using Falu.Core;

namespace Falu.Client.Money;

internal interface ISupportsUploadingMpesaStatement
{
    string ObjectKind { get; }

    Task<ResourceResponse<TResource>> RequestAsync<TResource>(string uri,
                                                              HttpMethod method,
                                                              HttpContent? content = null,
                                                              RequestOptions? options = null,
                                                              CancellationToken cancellationToken = default);
}
