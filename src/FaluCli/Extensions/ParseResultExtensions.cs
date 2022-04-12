namespace System.CommandLine.Parsing;

/// <summary>Extensions for <see cref="ParseResult"/>.</summary>
internal static class ParseResultExtensions
{
    // These extensions exist here as a workaround resulting from changes in beta2
    // A better way of binding should be sort out

    public static T? ValueForOption<T>(this ParseResult result, string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException($"'{nameof(alias)}' cannot be null or whitespace.", nameof(alias));
        }

        var opt = result.CommandResult.Command.FindOption<T>(alias);
        return opt is null ? default : result.GetValueForOption(opt);
    }

    public static T? ValueForArgument<T>(this ParseResult result, string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException($"'{nameof(alias)}' cannot be null or whitespace.", nameof(alias));
        }

        var arg = result.CommandResult.Command.FindArgument<T>(alias);
        return arg is null ? default : result.GetValueForArgument(arg);
    }

    private static Option<T>? FindOption<T>(this Command command, string alias)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        ArgumentNullException.ThrowIfNull(alias, nameof(alias));

        var opt = command.Options.FirstOrDefault(o => o.Aliases.Contains(alias));
        if (opt is not null && opt is Option<T> opt_t) return opt_t;

        var parent = command.Parents.OfType<Command>().SingleOrDefault();
        if (parent is not null) return FindOption<T>(parent, alias);
        return null;
    }

    private static Argument<T>? FindArgument<T>(this Command command, string name)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        ArgumentNullException.ThrowIfNull(name, nameof(name));

        var arg = command.Arguments.FirstOrDefault(o => o.Name.Equals(name));
        if (arg is not null && arg is Argument<T> arg_t) return arg_t;

        var parent = command.Parents.OfType<Command>().SingleOrDefault();
        if (parent is not null) return FindArgument<T>(parent, name);
        return null;
    }
}
