using Falu.Client;

namespace Falu.Commands.Login;

internal class LoginCommandHandler : ICommandHandler
{
    public FaluCliClient client;
    private readonly ILogger logger;

    public LoginCommandHandler(FaluCliClient client, ILogger<LoginCommandHandler> logger)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {

        return 0;
    }
}
