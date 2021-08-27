using Falu;

namespace FaluCli.Client
{
    internal class FaluCliClientOptions : FaluClientOptions
    {
        public string? WorkspaceId { get; set; }
        public bool? Live { get; set; }
    }
}
