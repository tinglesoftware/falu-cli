using Falu.Core;
using Falu.MessageTemplates;

namespace FaluCli.Commands.Templates
{
    internal class TemplateInfo : IHasDescription, IHasMetadata
    {
        public TemplateInfo() { } // required for deserialization

        public TemplateInfo(MessageTemplate template)
        {
            Alias = template.Alias;
            Description = template.Alias;
            Metadata = template.Metadata;
        }

        public string? Alias { get; set; }

        /// <inheritdoc/>
        public string? Description { get; set; }

        /// <inheritdoc/>
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
