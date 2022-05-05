using Falu.Client;
using Falu.Messages;

namespace Falu.Commands.Messages;

internal class SendMessagesCommandHandler : ICommandHandler
{
    public FaluCliClient client;
    private readonly ILogger logger;

    public SendMessagesCommandHandler(FaluCliClient client, ILogger<SendMessagesCommandHandler> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();
        var tos = context.ParseResult.ValueForOption<string[]>("--to")!;
        var stream = context.ParseResult.ValueForOption<string>("--stream")!;

        var command = context.ParseResult.CommandResult.Command;
        if (command is SendRawMessagesCommand) return HandleRawAsync(context, tos, stream, cancellationToken);
        else if (command is SendTemplatedMessagesCommand) return HandleTemplatedAsync(context, tos, stream, cancellationToken);
        throw new InvalidOperationException($"Command of type '{command.GetType().FullName}' is not supported here.");
    }

    private async Task<int> HandleRawAsync(InvocationContext context, string[] tos, string stream, CancellationToken cancellationToken)
    {
        var body = context.ParseResult.ValueForOption<string>("--body");

        var requests = CreateRequests(tos, stream, r => r.Body = body);
        await SendMessagesAsync(requests, cancellationToken);
        return 0;
    }

    private async Task<int> HandleTemplatedAsync(InvocationContext context, string[] tos, string stream, CancellationToken cancellationToken)
    {
        var id = context.ParseResult.ValueForOption<string>("--id");
        var alias = context.ParseResult.ValueForOption<string>("--alias");
        var model = System.Text.Json.Nodes.JsonNode.Parse(context.ParseResult.ValueForOption<string>("--model")!);

        // ensure we both id and alias are not null
        if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(alias))
        {
            logger.LogError("A template identifier or template alias must be provided when sending a templated message.");
            return -1;
        }

        var requests = CreateRequests(tos, stream, r => r.Template = new MessageSourceTemplate { Id = id, Alias = alias, Model = model, });
        await SendMessagesAsync(requests, cancellationToken);
        return 0;
    }

    private async Task SendMessagesAsync(IList<MessageCreateRequest> requests, CancellationToken cancellationToken)
    {
        var rr = await client.Messages.SendBatchAsync(requests, cancellationToken: cancellationToken);
        rr.EnsureSuccess();

        var response = rr.Resource!;
        var scheduled = response.Created;
        var ids = response.Ids!;
        logger.LogInformation("Message(s) scheduled for sending at {Scheduled:r}.", scheduled);
        logger.LogDebug("Message Id(s):\r\n-{Ids}", string.Join("\r\n-", ids));
    }

    private static IList<MessageCreateRequest> CreateRequests(string[] tos, string stream, Action<MessageCreateRequest> setupFunc)
    {
        ArgumentNullException.ThrowIfNull(tos);
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(setupFunc);

        // a maximum of 500 tos per batch
        var groups = tos.Chunk(500).ToList();
        var results = new List<MessageCreateRequest>(groups.Count);
        foreach (var group in groups)
        {
            var request = new MessageCreateRequest { To = group, Stream = stream, };
            setupFunc(request);
        }
        return results;
    }
}
