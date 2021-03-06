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

    int ICommandHandler.Invoke(InvocationContext context) => throw new NotImplementedException();

    public Task<int> InvokeAsync(InvocationContext context)
    {
        // ensure both to and file are not null or empty
        var tos = context.ParseResult.ValueForOption<string[]>("--to");
        var filePath = context.ParseResult.ValueForOption<string>("--file");
        if ((tos is null || tos.Length == 0) && string.IsNullOrWhiteSpace(filePath))
        {
            logger.LogError("A CSV file path must be specified or the destinations using the --to option.");
            return Task.FromResult(-1);
        }

        // ensure both to and file are not specified
        if (tos is not null && tos.Length > 0 && !string.IsNullOrWhiteSpace(filePath))
        {
            logger.LogError("Either specify the CSV file path or destinations not both.");
            return Task.FromResult(-1);
        }

        // read the numbers from the CSV file
        if (tos is null || tos.Length == 0)
        {
            tos = File.ReadAllText(filePath!)
                      .Replace("\r\n", ",")
                      .Replace("\r", ",")
                      .Replace("\n", ",")
                      .Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        var stream = context.ParseResult.ValueForOption<string>("--stream")!;
        var cancellationToken = context.GetCancellationToken();

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
        var model = System.Text.Json.JsonSerializer.Deserialize<IDictionary<string, object>>(context.ParseResult.ValueForOption<string>("--model")!);

        // ensure both id and alias are not null
        if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(alias))
        {
            logger.LogError("A template identifier or template alias must be provided when sending a templated message.");
            return -1;
        }

        // ensure both id and alias are not specified
        if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(alias))
        {
            logger.LogError("Either specify the template identifier or template alias not both.");
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
        logger.LogInformation("Scheduled {Count} for sending at {Scheduled:r}.", ids.Count, scheduled);
        logger.LogDebug("Message Id(s):\r\n-{Ids}", string.Join("\r\n-", ids));
    }

    private static IList<MessageCreateRequest> CreateRequests(string[] tos, string stream, Action<MessageCreateRequest> setupFunc)
    {
        ArgumentNullException.ThrowIfNull(tos);
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(setupFunc);

        // a maximum of 500 tos per batch
        var groups = tos.Distinct(StringComparer.OrdinalIgnoreCase).Chunk(500).ToList();
        var requests = new List<MessageCreateRequest>(groups.Count);
        foreach (var group in groups)
        {
            var request = new MessageCreateRequest { To = group, Stream = stream, };
            setupFunc(request);
            requests.Add(request);
        }
        return requests;
    }
}
