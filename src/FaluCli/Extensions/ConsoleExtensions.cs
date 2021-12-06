namespace System.CommandLine.IO;

/// <summary>
/// Extension methods for <see cref="IConsole"/>
/// </summary>
internal static class ConsoleExtensions
{
    internal static void SetTerminalForegroundRed(this IConsole console)
    {
        if (console.GetType().GetInterfaces().Any(i => i.Name == "ITerminal"))
        {
            ((dynamic)console).ForegroundColor = ConsoleColor.Red;
        }

        if (Platform.IsConsoleRedirectionCheckSupported &&
            !Console.IsOutputRedirected)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (Platform.IsConsoleRedirectionCheckSupported)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }

    internal static void ResetTerminalForegroundColor(this IConsole console)
    {
        if (console.GetType().GetInterfaces().Any(i => i.Name == "ITerminal"))
        {
            ((dynamic)console).ForegroundColor = ConsoleColor.Red;
        }

        if (Platform.IsConsoleRedirectionCheckSupported &&
            !Console.IsOutputRedirected)
        {
            Console.ResetColor();
        }
        else if (Platform.IsConsoleRedirectionCheckSupported)
        {
            Console.ResetColor();
        }
    }
}

/// <summary>
/// Extension methods for <see cref="IStandardStreamWriter"/>
/// </summary>
public static class IStandardStreamWriterExtensions
{
    public static void WriteLine(this IStandardStreamWriter writer, string format, params object?[] args)
    {
        writer.WriteLine(string.Format(format, args));
    }
}
