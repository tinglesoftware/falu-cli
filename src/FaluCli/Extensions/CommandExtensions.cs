namespace System.CommandLine;

/// <summary>
/// Extension methods on <see cref="Command"/>.
/// </summary>
public static class CommandExtensions
{
    #region Options

    ///
    public static Command AddOption<T>(this Command command,
                                       IEnumerable<string> aliases,
                                       string? description = null,
                                       ValidateSymbolResult<OptionResult>? validate = null,
                                       Action<Option<T>>? configure = null)
    {
        // Create the option and add it to the command
        var option = CreateOption(aliases: aliases,
                                  description: description,
                                  validate: validate,
                                  configure: configure);

        command.AddOption(option);
        return command;
    }

    ///
    public static Command AddOption<T>(this Command command,
                                       IEnumerable<string> aliases,
                                       string? description,
                                       T defaultValue,
                                       ValidateSymbolResult<OptionResult>? validate = null,
                                       Action<Option<T>>? configure = null)
    {
        // Create the option and add it to the command
        var option = CreateOption(aliases: aliases,
                                  description: description,
                                  validate: validate,
                                  configure: configure);

        // Set default value
        option.SetDefaultValue(defaultValue);

        command.AddOption(option);
        return command;
    }

    ///
    public static Command AddGlobalOption<T>(this Command command,
                                             IEnumerable<string> aliases,
                                             string? description,
                                             T defaultValue,
                                             ValidateSymbolResult<OptionResult>? validate = null,
                                             Action<Option<T>>? configure = null)
    {
        // Create the option and add it to the command
        var option = CreateOption(aliases: aliases,
                                  description: description,
                                  validate: validate,
                                  configure: configure);

        // Set default value
        option.SetDefaultValue(defaultValue);

        command.AddGlobalOption(option);
        return command;
    }

    ///
    public static Command AddGlobalOption<T>(this Command command,
                                             IEnumerable<string> aliases,
                                             string? description = null,
                                             ValidateSymbolResult<OptionResult>? validate = null,
                                             Action<Option<T>>? configure = null)
    {
        // Create the option and add it to the command
        var option = CreateOption(aliases: aliases,
                                  description: description,
                                  validate: validate,
                                  configure: configure);

        command.AddGlobalOption(option);
        return command;
    }

    private static Option<T> CreateOption<T>(IEnumerable<string> aliases,
                                             string? description = null,
                                             ValidateSymbolResult<OptionResult>? validate = null,
                                             Action<Option<T>>? configure = null)
    {
        // Create the option
        var option = new Option<T>(aliases: aliases.ToArray(), description: description);

        // Add validator if provided
        if (validate is not null)
        {
            option.AddValidator(validate);
        }

        // Perfom further configuration
        configure?.Invoke(option);

        return option;
    }

    #endregion

    #region Arguments

    ///
    public static Command AddArgument<T>(this Command command,
                                         string name,
                                         string? description = null,
                                         ValidateSymbolResult<ArgumentResult>? validate = null,
                                         Action<Argument<T>>? configure = null)
    {
        // Create the argument and add it to the command
        var argument = CreateArgument(name: name,
                                      description: description,
                                      validate: validate,
                                      configure: configure);

        command.AddArgument(argument);
        return command;
    }

    ///
    public static Command AddArgument<T>(this Command command,
                                         string name,
                                         string? description,
                                         T defaultValue,
                                         ValidateSymbolResult<ArgumentResult>? validate = null,
                                         Action<Argument<T>>? configure = null)
    {
        // Create the argument and add it to the command
        var argument = CreateArgument(name: name,
                                      description: description,
                                      validate: validate,
                                      configure: configure);

        // Set default value if provided
        argument.SetDefaultValue(defaultValue);

        command.AddArgument(argument);
        return command;
    }

    private static Argument<T> CreateArgument<T>(string name,
                                                 string? description = null,
                                                 ValidateSymbolResult<ArgumentResult>? validate = null,
                                                 Action<Argument<T>>? configure = null)
    {
        // Create the argument
        var argument = new Argument<T>(name: name, description: description);

        // Add validator if provided
        if (validate is not null)
        {
            argument.AddValidator(validate);
        }

        // Perfom further configuration
        configure?.Invoke(argument);

        return argument;
    }

    #endregion

    public static Command AddCommonGlobalOptions(this Command command)
    {
        // TODO: validate workspaceId using regex -> "^wksp_[0-9a-f]{24}$"
        command.AddGlobalOption<string>(aliases: new[] { "--workspace", },
                                        description: "The identifier of the workspace being accessed. Required when login is by user account. Example: wksp_610010be9228355f14ce6e08");

        command.AddGlobalOption<bool>(aliases: new[] { "--live", },
                                      description: "Whether the entity resides in live mode or not. Required when login is by user account.");

        // TODO: validate api key using regex -> "^{sk|pk}_{live|test}_[0-9a-zA-Z]+$"
        command.AddGlobalOption<string>(aliases: new[] { "-k", "--apikey", },
                                        description: "The API key to use for the command. Required it not logged in or when accessing another workspace. Looks like: sk_test_LdVyn0upN...");

        command.AddGlobalOption(new[] { "-v", "--verbose" }, "Whether to output verbosely.", false);

        return command;
    }
}
