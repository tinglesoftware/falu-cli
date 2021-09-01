using FaluCli.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaluCli.Commands.Templates
{
    internal class PushTemplatesCommandHandler : ICommandHandler
    {
        public FaluCliClient client;
        private readonly ILogger logger;

        public PushTemplatesCommandHandler(FaluCliClient client, ILogger<PushTemplatesCommandHandler> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            var all = context.ParseResult.ValueForOption<bool>("--all");

            return 0;
        }
    }
}
