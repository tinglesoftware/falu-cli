namespace System.CommandLine.IO;

/// <summary>
/// Extension methods for <see cref="IConsole"/>
/// </summary>
internal static class ConsoleExtensions
{
    internal static void SetTerminalForegroundRed(this IConsole console) => console.SetTerminalForegroundColor(ConsoleColor.Red);
    internal static void SetTerminalForegroundGreen(this IConsole console) => console.SetTerminalForegroundColor(ConsoleColor.Green);

    internal static void SetTerminalForegroundColor(this IConsole console, ConsoleColor color)
    {
        if (console.GetType().GetInterfaces().Any(i => i.Name == "ITerminal"))
        {
            ((dynamic)console).ForegroundColor = color;
        }

        if (Platform.IsConsoleRedirectionCheckSupported &&
            !Console.IsOutputRedirected)
        {
            Console.ForegroundColor = color;
        }
        else if (Platform.IsConsoleRedirectionCheckSupported)
        {
            Console.ForegroundColor = color;
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
