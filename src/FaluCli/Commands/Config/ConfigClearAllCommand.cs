namespace Falu.Commands.Config;

public class ConfigClearAllCommand : Command
{
    public ConfigClearAllCommand() : base("all", "Clear all cofiguration values by deleting the configuration file.")
    {
    }
}

public class ConfigClearAuthenticationCommand : Command
{
    public ConfigClearAuthenticationCommand() : base("authentication", "Clear cofiguration values related to authenticatin.")
    {
    }
}
