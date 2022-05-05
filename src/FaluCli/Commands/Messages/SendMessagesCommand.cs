namespace Falu.Commands.Messages;

public class SendMessagesCommand : WorkspacedCommand
{
    public SendMessagesCommand() : base("send", "Send messages.")
    {
        AddCommand(new SendRawMessagesCommand());
        AddCommand(new SendTemplatedMessagesCommand());
    }
}

public class SendRawMessagesCommand : Command
{
    public SendRawMessagesCommand() : base("raw", "Send a message with the body defined.")
    {
    }
}

public class SendTemplatedMessagesCommand : Command
{
    public SendTemplatedMessagesCommand() : base("raw", "Send a templated message.")
    {
    }
}
