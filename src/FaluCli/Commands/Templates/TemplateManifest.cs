namespace FaluCli.Commands.Templates
{
    internal class TemplateManifest
    {
        public TemplateManifest(TemplateInfo info, string body)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public TemplateInfo Info { get; }

        public string? Alias => Info.Alias;

        public string Body { get; }

        public ChangeType ChangeType { get; set; } = ChangeType.Unmodified;

        public string? Id { get; set; }
    }

    enum ChangeType
    {
        Unmodified,
        Added,
        Modified,
    }
}
