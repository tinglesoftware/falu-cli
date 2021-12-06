namespace System.CommandLine;

internal static class Platform
{
    private static readonly Lazy<bool> isConsoleRedirectionCheckSupported = new(() =>
    {
        try
        {
            var check = Console.IsOutputRedirected;
            return true;
        }

        catch (PlatformNotSupportedException)
        {
            return false;
        }
    });

    public static bool IsConsoleRedirectionCheckSupported => isConsoleRedirectionCheckSupported.Value;
}
