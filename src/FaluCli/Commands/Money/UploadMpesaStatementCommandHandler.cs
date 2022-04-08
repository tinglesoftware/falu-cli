using Falu.Client;

namespace Falu.Commands.Money;

internal class UploadMpesaStatementCommandHandler : ICommandHandler
{
    public FaluCliClient client;
    private readonly ILogger logger;

    public UploadMpesaStatementCommandHandler(FaluCliClient client, ILogger<UploadMpesaStatementCommandHandler> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();
        var filePath = context.ParseResult.ValueForOption<string>("--file");

        return 0;
    }
}
