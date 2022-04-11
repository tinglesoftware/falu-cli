namespace System.CommandLine.Parsing;

/// <summary>Extensions for <see cref="ParseResult"/>.</summary>
internal static class ParseResultExtensions
{
    // These extensions exist here as a workaround resuling from changes in beta2
    // A better way of binding should be sort out
    public static T? ValueForOption<T>(this ParseResult result, string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException($"'{nameof(alias)}' cannot be null or whitespace.", nameof(alias));
        }

        var optionResult = result.CommandResult.Children.OfType<OptionResult>().FirstOrDefault(or => or.Option.Aliases.Contains(alias));
        return optionResult is not null ? optionResult.GetValueOrDefault<T>() : default;
    }

    public static T? ValueForArgument<T>(this ParseResult result, string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException($"'{nameof(alias)}' cannot be null or whitespace.", nameof(alias));
        }

        var argumentResult = result.CommandResult.Children.OfType<ArgumentResult>().FirstOrDefault(ar => ar.Argument.Name.Equals(alias));
        return argumentResult is not null ? argumentResult.GetValueOrDefault<T>() : default;
    }
}
