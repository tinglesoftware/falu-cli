namespace FaluCli.Client;

internal class FaluCliClientHandler : DelegatingHandler
{
    private readonly InvocationContext context;

    public FaluCliClientHandler(InvocationContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AddHeaders(request, context);
        return base.SendAsync(request, cancellationToken);
    }

    /// <inheritdoc/>
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AddHeaders(request, context);
        return base.Send(request, cancellationToken);
    }

    private static void AddHeaders(HttpRequestMessage request, InvocationContext context)
    {
        var workspaceId = context.ParseResult.ValueForOption<string>("--workspace");
        var live = context.ParseResult.ValueForOption<bool?>("--live");

        if (!string.IsNullOrWhiteSpace(workspaceId))
        {
            request.Headers.Add("X-Workspace-Id", workspaceId);
        }

        if (live is not null)
        {
            request.Headers.Add("X-Live-Mode", live.Value.ToString().ToLowerInvariant());
        }
    }
}
