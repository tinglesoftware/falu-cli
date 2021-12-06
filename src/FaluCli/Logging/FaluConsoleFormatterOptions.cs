using Microsoft.Extensions.Logging.Console;

namespace FaluCli.Logging;

public class FaluConsoleFormatterOptions : SimpleConsoleFormatterOptions
{
    /// <summary>Includes category when true.</summary>
    public bool IncludeCategory { get; set; } = true;

    /// <summary>Includes event id when true.</summary>
    public bool IncludeEventId { get; set; } = true;
}
