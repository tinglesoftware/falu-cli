namespace System.CommandLine;

/// <summary>
/// Extensions for <see cref="InvocationContext"/>
/// </summary>
internal static class InvocationContextExtensions
{
    public static bool IsVerboseEnabled(this InvocationContext context)
    {
        return context.ParseResult.ValueForOption<bool>("--verbose");
    }
}
