using Falu;
using System.CommandLine.IO;
using System.Net;
using Res = Falu.Properties.Resources;

namespace System.CommandLine.Builder;

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

        if (exception is FaluException fe)
        {
            var error = fe.Error;
            if (error is not null)
            {
                stderr.WriteLine(Res.RequestFailedHeader);
                stderr.WriteLine();
                stderr.WriteLine(Res.RequestIdFormat, fe.RequestId);
                if (!string.IsNullOrWhiteSpace(fe.TraceId))
                {
                    stderr.WriteLine(Res.TraceIdentifierFormat, fe.TraceId);
                }

                stderr.WriteLine(Res.ProblemDetailsErrorCodeFormat, error.Title);
                if (!string.IsNullOrWhiteSpace(error.Detail))
                {
                    stderr.WriteLine(Res.ProblemDetailsErrorDetailFormat, error.Detail);
                }
            }
            else if (fe.StatusCode == HttpStatusCode.Unauthorized)
            {
                stderr.WriteLine(Res.Unauthorized401ErrorMessage);
            }
            else if (fe.StatusCode == HttpStatusCode.Forbidden)
            {
                stderr.WriteLine(Res.Forbidden403Message);
            }
            else
            {
                stderr.WriteLine(fe.Message);
            }
        }
        else
        {
            stderr.WriteLine(Res.UnhandledExceptionFormat, exception.ToString());
        }

        console.ResetTerminalForegroundColor();
    }
}
