namespace Falu.Commands.Login;

internal class LoginCommand : Command
{
    public LoginCommand() : base("login", "Login to your Falu account to setup the CLI")
    {
        this.AddOption(new[] { "--no-browser", },
                       description: "Set true to not open the browser automatically for authentication.",
                       defaultValue: false);

        //this.AddOption(new[] { "-i", "--interactive", },
        //               description: "Run interactive configuration mode if you cannot open a browser.",
        //               defaultValue: false);
    }
}

internal class LogoutCommand : Command
{
    public LogoutCommand() : base("logout", "Logout of your Falu account from the CLI")
    {
        //this.AddOption(new[] { "-a", "--all", },
        //               description: " Clear credentials for all workspaces you are currently logged into.",
        //               defaultValue: false);
    }
}
