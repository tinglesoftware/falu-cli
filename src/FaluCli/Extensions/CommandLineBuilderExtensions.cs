using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Net;

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

            var stderr = console.Error;

            if (exception is Falu.Infrastructure.FaluException fe)
            {
                var error = fe.Error;
                if (error is not null)
                {
                    stderr.WriteLine($"RequestId: {fe.RequestId}");
                    if (!string.IsNullOrWhiteSpace(fe.TraceId))
                    {
                        stderr.WriteLine($"TraceId: {fe.TraceId}");
                    }

                    stderr.WriteLine($"Code: {error.Title!}");
                    if (!string.IsNullOrWhiteSpace(error.Detail))
                    {
                        stderr.WriteLine(error.Detail);
                    }
                }
                else if (fe.StatusCode == HttpStatusCode.Unauthorized)
                {
                    stderr.WriteLine("The API key provided cannot authenticate your request.\r\nConfirm you have provided the right value and that it matches any other related values.\r\nFor example, do not use a key that belongs to another workspace or mix live and test mode keys");
                }
                else if (fe.StatusCode == HttpStatusCode.Forbidden)
                {
                    stderr.WriteLine("The API Key provided or logged in user does not have permissions to perform this operation.\r\nConsult your Falu dashboard to confirm the permissions of the API key or ask your administrator to grant you permissions.");
                }
                else
                {
                    stderr.WriteLine(fe.Message);
                }
            }
            else
            {
                stderr.WriteLine($"Unhandled exception: {exception}");
            }

            console.ResetTerminalForegroundColor();
        }
    }
}
