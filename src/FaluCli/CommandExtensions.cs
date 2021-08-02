using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Linq;

namespace System.CommandLine
{
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
                                           Func<T>? getDefaultValue = null,
                                           ValidateSymbol<OptionResult>? validate = null,
                                           Action<Option<T>>? configure = null)
        {
            // Create the option and add it to the command
            var option = CreateOption(aliases: aliases,
                                      description: description,
                                      getDefaultValue: getDefaultValue,
                                      validate: validate,
                                      configure: configure);
            command.AddOption(option);
            return command;
        }

        ///
        public static Command AddGlobalOption<T>(this Command command,
                                                 IEnumerable<string> aliases,
                                                 string? description = null,
                                                 Func<T>? getDefaultValue = null,
                                                 ValidateSymbol<OptionResult>? validate = null,
                                                 Action<Option<T>>? configure = null)
        {
            // Create the option and add it to the command
            var option = CreateOption(aliases: aliases,
                                      description: description,
                                      getDefaultValue: getDefaultValue,
                                      validate: validate,
                                      configure: configure);
            command.AddGlobalOption(option);
            return command;
        }

        private static Option<T> CreateOption<T>(IEnumerable<string> aliases,
                                                 string? description = null,
                                                 Func<T>? getDefaultValue = null,
                                                 ValidateSymbol<OptionResult>? validate = null,
                                                 Action<Option<T>>? configure = null)
        {
            // Create the option
            Option<T> option = getDefaultValue is null
                ? new Option<T>(aliases: aliases.ToArray(),
                                description: description)
                : new Option<T>(aliases: aliases.ToArray(),
                                description: description,
                                getDefaultValue: getDefaultValue);

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
                                             Func<T>? getDefaultValue = null,
                                             ValidateSymbol<ArgumentResult>? validate = null,
                                             Action<Argument<T>>? configure = null)
        {
            // Create the argument and add it to the command
            var argument = CreateArgument(name: name,
                                          description: description,
                                          getDefaultValue: getDefaultValue,
                                          validate: validate,
                                          configure: configure);
            command.AddArgument(argument);
            return command;
        }

        private static Argument<T> CreateArgument<T>(string name,
                                                 string? description = null,
                                                 Func<T>? getDefaultValue = null,
                                                 ValidateSymbol<ArgumentResult>? validate = null,
                                                 Action<Argument<T>>? configure = null)
        {
            // Create the argument
            Argument<T> option = getDefaultValue is null
                ? new Argument<T>(name: name,
                                  description: description)
                : new Argument<T>(name: name,
                                  description: description,
                                  getDefaultValue: getDefaultValue);

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

        public static Command AddCommonServiceBusOptions(this Command command)
        {
            command.AddOption<string>(aliases: new[] { "-c", "--connection-string", },
                                      description: "The connection string of the Service Bus instance.");

            command.AddOption(aliases: new[] { "--namespace", },
                              description: "The Fully Qualified namespace of the Service Bus instance.",
                              getDefaultValue: () => "falu-prod.servicebus.windows.net");

            return command;
        }
    }
}
