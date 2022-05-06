using PhoneNumbers;
using Res = Falu.Properties.Resources;

namespace Falu.Commands.Messages;

public abstract class AsbtractSendMessagesCommand : Command
{
    public AsbtractSendMessagesCommand(string name, string? description = null) : base(name, description)
    {
        this.AddOption<string[]>(new[] { "--to", "-t", },
                                 description: "Phone number(s) you are sending to, in E.164 format.",
                                 validate: (or) => or.ErrorMessage = ValidateNumbers(or.Option.Name, or.GetValueOrDefault<string[]>()!));

        this.AddOption<string>(new[] { "-f", "--file", },
                               description: "File path for the path containing the phone numbers you are sending to, in E.164 format."
                                          + " The file should have no headers, all values on one line, separated by commas.",
                               validate: (or) =>
                               {
                                   // ensure the file exists
                                   var value = or.GetValueOrDefault<string>()!;
                                   var info = new FileInfo(value);
                                   if (!info.Exists)
                                   {
                                       or.ErrorMessage = $"The file {value} does not exist.";
                                       return;
                                   }

                                   var numbers = File.ReadAllText(value).Split(',', StringSplitOptions.RemoveEmptyEntries);
                                   or.ErrorMessage = ValidateNumbers(or.Option.Name, numbers);
                               });

        this.AddOption(new[] { "--stream", "-s", },
                       description: "The stream to use, either the name or unique identifier. Example: mstr_610010be9228355f14ce6e08 or transactional",
                       defaultValue: "transactional",
                       configure: o => o.IsRequired = true);
    }

    private static string? ValidateNumbers(string optionName, string[] numbers)
    {
        // ensure not more than 500*1000 (500 per batch and 1000 batches per request)
        var limit = 500_000;
        if (numbers.Length > limit)
        {
            return string.Format(Res.TooManyMessagesToBeSent, limit);
        }

        // ensure each value is in E.164 format
        var util = PhoneNumberUtil.GetInstance();
        foreach (var n in numbers)
        {
            try
            {
                _ = util.Parse(n, null);
            }
            catch (Exception ex) when (ex is NumberParseException)
            {
                return string.Format(Res.InvalidE164PhoneNumber, optionName, n);
            }
        }

        return null;
    }
}

public class SendRawMessagesCommand : AsbtractSendMessagesCommand
{
    public SendRawMessagesCommand() : base("raw", "Send a message with the body defined.")
    {
        this.AddOption<string>(new[] { "--body", },
                               description: "The actual message content to be sent.",
                               configure: o => o.IsRequired = true);
    }
}

public class SendTemplatedMessagesCommand : AsbtractSendMessagesCommand
{
    public SendTemplatedMessagesCommand() : base("templated", "Send a templated message.")
    {
        this.AddOption(new[] { "--id", "-i", },
                       description: "The unique template identifier. Example: mtpl_610010be9228355f14ce6e08",
                       format: Constants.MessageTemplateIdFormat);

        this.AddOption(new[] { "--alias", "-a", },
                       description: "The template alias, unique to your workspace.",
                       format: Constants.MessageTemplateAliasFormat);

        this.AddOption(new[] { "--model", "-m", },
                       description: "The model to use with the template. Example --model '{\"name\": \"John\"}'",
                       defaultValue: "{}",
                       validate: (or) =>
                       {
                           var value = or.GetValueOrDefault<string>()!;
                           try
                           {
                               System.Text.Json.JsonSerializer.Deserialize<IDictionary<string, object>>(value);
                           }
                           catch (System.Text.Json.JsonException)
                           {
                               or.ErrorMessage = string.Format(Res.InvalidJsonInputValue, or.Option.Name);
                           }
                       },
                       configure: o => o.IsRequired = true);
    }
}
