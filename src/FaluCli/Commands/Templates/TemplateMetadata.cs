using Falu.Core;
using Falu.MessageTemplates;
using System.Collections.Generic;

namespace FaluCli.Commands.Templates
{
    internal class TemplateMetadata : IHasId, IHasDescription, IHasMetadata
    {
        public TemplateMetadata() { } // required for deserialization

        public TemplateMetadata(MessageTemplate template)
        {
            Id = template.Id;
            Alias = template.Alias;
            Description = template.Alias;
            Metadata = template.Metadata;
        }

        /// <inheritdoc/>
        public string? Id { get; set; }

        public string? Alias { get; set; }

        /// <inheritdoc/>
        public string? Description { get; set; }

        /// <inheritdoc/>
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
