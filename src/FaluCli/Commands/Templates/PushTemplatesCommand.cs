namespace FaluCli.Commands.Templates;

public class PushTemplatesCommand : Command
{
    public PushTemplatesCommand() : base("push", "Pushes changed templates from the local file system to Falu servers.")
    {
        this.AddArgument<string>(name: "templates-directory",
                                 description: "The directory containing the templates.");

        this.AddOption(new[] { "-a", "--all", },
                       description: "Push all local templates up to Falu regardless of whether they changed.",
                       defaultValue: false);
    }
}
