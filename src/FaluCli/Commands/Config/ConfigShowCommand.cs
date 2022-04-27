namespace Falu.Commands.Config;

public class ConfigShowCommand : Command
{
    public ConfigShowCommand() : base("show", "Show present cofiguration values.")
    {
    }
}

public class ConfigSetCommand : Command
{
    public ConfigSetCommand() : base("set", "Set a cofiguration value.")
    {
        this.AddArgument<string>(name: "key",
                                 description: "The configuration key.",
                                 configure: a => a.FromAmong("workspace", "livemode"));

        this.AddArgument<string>(name: "value", description: "The configuration value.");
    }
}

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
