namespace Falu;

public class WorkspacedCommand : Command
{
    public WorkspacedCommand(string name, string? description = null) : base(name, description)
    {
        this.AddGlobalOption(aliases: new[] { "--apikey", },
                             description: "The API key to use for the command. Required it not logged in or when accessing another workspace. Looks like: sk_test_LdVyn0upN...",
                             format: Constants.ApiKeyFormat);

        this.AddGlobalOption(aliases: new[] { "--workspace", },
                             description: "The identifier of the workspace being accessed. Required when login is by user account. Example: wksp_610010be9228355f14ce6e08",
                             format: Constants.WorkspaceIdFormat);

        // without this the nullable type, the option is not found because we have not migrated to the new bindings
        this.AddGlobalOption<bool?>(aliases: new[] { "--live", },
                                    description: "Whether the entity resides in live mode or not. Required when login is by user account.");
    }
}
