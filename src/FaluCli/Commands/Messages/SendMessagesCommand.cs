using PhoneNumbers;
using Res = Falu.Properties.Resources;

namespace Falu.Commands.Messages;

public abstract class AsbtractSendMessagesCommand : Command
{
    public AsbtractSendMessagesCommand(string name, string? description = null) : base(name, description)
    {
        this.AddOption<string[]>(new[] { "-t", "--to", },
                                 description: "Phone number(s) you are sending to, in E.164 format.",
                                 validate: (or) =>
                                 {
                                     var util = PhoneNumberUtil.GetInstance();
                                     var value = or.GetValueOrDefault<string[]>()!;
                                     foreach (var v in value)
                                     {
                                         try
                                         {
                                             _ = util.Parse(v, null);
                                         }
                                         catch (Exception ex) when (ex is NumberParseException)
                                         {
                                             or.ErrorMessage = string.Format(Res.InvalidE164PhoneNumber, or.Option.Name, v);
                                             return;
                                         }
                                     }
                                 },
                                 configure: o => o.IsRequired = true);

        this.AddOption(new[] { "-s", "--stream", },
                       description: "The stream to use, either the name or unique identifier. Example: mstr_610010be9228355f14ce6e08 or transactional",
                       defaultValue: "transactional",
                       configure: o => o.IsRequired = true);
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
    public SendTemplatedMessagesCommand() : base("template", "Send a templated message.")
    {
        this.AddOption(new[] { "-i", "--id", },
                       description: "The unique template identifier. Example: mtpl_610010be9228355f14ce6e08",
                       format: Constants.MessageTemplateIdFormat);

        this.AddOption(new[] { "-a", "--alias", },
                       description: "The template alias, unique to your workspace.",
                       format: Constants.MessageTemplateAliasFormat);

        this.AddOption(new[] { "-m", "--model", },
                       description: "The model to use with the template. Example --model '{\"name\": \"John\"}'",
                       defaultValue: "{}",
                       validate: (or) =>
                       {
                           var value = or.GetValueOrDefault<string>()!;
                           try
                           {
                               var node = System.Text.Json.Nodes.JsonNode.Parse(value);
                               if (node is not System.Text.Json.Nodes.JsonObject jo)
                               {
                                   or.ErrorMessage = string.Format(Res.JsonInputShouldBeOfType, or.Option.Name, "object");
                               }
                           }
                           catch (System.Text.Json.JsonException)
                           {
                               or.ErrorMessage = string.Format(Res.InvalidJsonInputValue, or.Option.Name);
                           }
                       },
                       configure: o => o.IsRequired = true);
    }
}
