using System.CommandLine.Invocation;
using System.CommandLine.IO;

namespace System.CommandLine.Builder
{
    /// <summary>
    /// Extensions for <see cref="CommandLineBuilder"/>
    /// </summary>
    internal static class CommandLineBuilderExtensions
    {
        public static CommandLineBuilder UseFaluDefaults(this CommandLineBuilder builder)
        {
            return builder.UseVersionOption()
                          .UseHelp()
                          .UseEnvironmentVariableDirective()
                          .UseParseDirective()
                          .UseDebugDirective()
                          .UseSuggestDirective()
                          .RegisterWithDotnetSuggest()
                          .UseTypoCorrections()
                          .UseParseErrorReporting()
                          .UseExceptionHandler(ExceptionHandler)
                          .CancelOnProcessTermination();
        }

        private static void ExceptionHandler(Exception exception, InvocationContext context)
        {
            context.ExitCode = 1;

            if (exception is OperationCanceledException) return;

            var console = context.Console;
            console.ResetTerminalForegroundColor();
            console.SetTerminalForegroundRed();

            if (exception is Falu.Infrastructure.FaluException fe)
            {
                var error = fe.Error;
                if (error is not null)
                {
                    console.Error.WriteLine($"RequestId: {fe.RequestId}");
                    console.Error.WriteLine($"TraceId: {fe.TraceId}");
                    console.Error.WriteLine(error.Title!);
                    console.Error.WriteLine(error.Detail!);
                }
                else
                {
                    console.Error.WriteLine(fe.Message);
                }
            }
            else
            {
                console.Error.WriteLine($"Unhandled exception: {exception}");
            }

            console.ResetTerminalForegroundColor();
        }
    }
}
