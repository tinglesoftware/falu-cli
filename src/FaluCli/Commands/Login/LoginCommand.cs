namespace Falu.Commands.Login;

internal class LoginCommand : Command
{
    public LoginCommand() : base("login", "Login to your Falu account to setup the CLI")
    {
        this.AddOption(new[] { "-i", "--interactive", },
                       description: "Run interactive configuration mode if you cannot open a browser.",
                       defaultValue: false);

    }
}
