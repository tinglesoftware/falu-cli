using System.Reflection;

namespace Falu.Updates;

internal static class VersioningHelper
{
    // get the version from the assembly
    private static readonly Lazy<string> _productVersion = new(delegate
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var attr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return attr is null ? assembly.GetName().Version!.ToString() : attr.InformationalVersion;
    });

    public static string ProductVersion => _productVersion.Value;
}
