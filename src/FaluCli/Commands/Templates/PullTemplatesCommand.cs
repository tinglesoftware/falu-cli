using System.CommandLine;

namespace FaluCli.Commands.Templates
{
    public class PullTemplatesCommand : Command
    {
        public PullTemplatesCommand() : base("pull", "Download templates from Falu servers to your local file system.")
        {
            this.AddArgument<string>(name: "output-directory",
                                     description: "The directory into which to put the pulled templates.");

            this.AddOption(new[] { "-o", "--overwrite", },
                           description: "Overwrite templates if they already exist.",
                           defaultValue: false);
        }
    }
}
