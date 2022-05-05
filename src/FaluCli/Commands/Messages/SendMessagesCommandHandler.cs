using Falu.Client;

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
        throw new NotImplementedException();
    }
}
